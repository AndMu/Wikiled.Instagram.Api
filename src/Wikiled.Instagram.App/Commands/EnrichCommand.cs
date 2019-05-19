using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Extensions;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Hashtags;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class EnrichCommand : DiscoveryCommand
    {
        private readonly ILogger<EnrichCommand> log;

        private readonly EnrichConfig config;

        private readonly IInstaApi instagram;

        private readonly ISmartTagsManager tagsManager;

        private ICaptionHandler captionHandler;

        private readonly string location;

        private ConcurrentDictionary<string, string> processed = new ConcurrentDictionary<string, string>();

        public EnrichCommand(ILogger<EnrichCommand> log, IInstaApi instagram, EnrichConfig config, ISessionHandler session, ISmartTagsManager tagsManager, ICaptionHandler captionHandler)
            : base(log, instagram, config, session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            location = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.instagram = instagram ?? throw new ArgumentNullException(nameof(instagram));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.tagsManager = tagsManager ?? throw new ArgumentNullException(nameof(tagsManager));
            this.captionHandler = captionHandler ?? throw new ArgumentNullException(nameof(captionHandler));
        }

        protected override async Task Internal(CurrentUser currentUser, CancellationToken token)
        {
            do
            {
                var media = await instagram.UserProcessor
                    .GetUserMediaById(currentUser.Pk, PaginationParameters.MaxPagesToLoad(1))
                    .ToArray();
                foreach (var item in media)
                {
                    log.LogInformation("Processing post - <{0}>", item.Identifier);
                    if (item.TakenAt.Date != DateTime.Today)
                    {
                        log.LogInformation("Stop processing old photos");
                        break;
                    }

                    if (processed.ContainsKey(item.Identifier))
                    {
                        log.LogInformation("Post <{0}> has been processed already", item.Identifier);
                        continue;
                    }

                    processed[item.Identifier] = item.Identifier;
                    await ProcessPost(item).ConfigureAwait(false);
                }

                log.LogDebug("Waiting for next cycle");
                await Task.Delay(TimeSpan.FromMinutes(5), token).ConfigureAwait(false);
            } while (!token.IsCancellationRequested);
        }

        private async Task ProcessPost(InstaMedia item)
        {
            log.LogInformation(
                "Processing media [{0}] - [{1}] [{2}] with Tags: {3}",
                item.Caption?.Text,
                item.DeviceTimeStamp,
                item.Location?.ShortName,
                item.UserTags.Count);

            var image = item.Images.OrderByDescending(x => x.Height).FirstOrDefault();
            if (image == null)
            {
                log.LogWarning("Image not found");
                return;
            }

            var caption = await GenerateCaption(item).ConfigureAwait(false);
            if (string.IsNullOrEmpty(caption))
            {
                log.LogInformation("Caption has not been generated");
                return;
            }

            var fileName = await DownloadImage(item, image).ConfigureAwait(false);
            await instagram.MediaProcessor.DeleteMediaAsync(item.Identifier, item.MediaType).ConfigureAwait(false);
            await UploadImage(fileName, caption, item.Location).ConfigureAwait(false);
            File.Delete(fileName);
        }

        private async Task<string> DownloadImage(InstaMedia item, InstaImage image)
        {
            var request = WebRequest.Create(image.Uri);
            var fileName = Path.Combine(location, $"{item.Identifier}.jpg");
            using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (FileStream streamWriter = File.Create(fileName))
                    {
                        await stream.CopyToAsync(streamWriter).ConfigureAwait(false);
                    }
                }
            }

            return fileName;
        }

        private async Task<string> GenerateCaption(InstaMedia item)
        {
            log.LogInformation("Generating caption...");
            var captionHolder = captionHandler.Extract(item.Caption?.Text);
            log.LogInformation("Adding [{0}] caption tags", captionHolder.Tags.Count());
            var tags = new HashSet<string>(captionHolder.Tags, StringComparer.OrdinalIgnoreCase);
            
            if (tags.Count > 20)
            {
                log.LogInformation("Found more than 20 tags on photo - ignoring it");
                return null;
            }

            if (item.Location != null)
            {
                var locationTags = await tagsManager.GetByLocationSmart(item.Location).ConfigureAwait(false);
                log.LogInformation("Adding [{0}] location tags", locationTags.Length);
                foreach (var tag in locationTags)
                {
                    tags.Add(tag);
                }
            }

            if (tags.Count == 0)
            {
                log.LogInformation("No tags found return original", captionHolder.Original);
            }

            var newTags = await tagsManager.GetSmart(27, tags.ToArray()).ConfigureAwait(false);
            captionHolder.AddTags(newTags);
            return captionHolder.Generate();
        }

        private Task UploadImage(string image, string caption, Location location)
        {
            log.LogInformation("Uploading Image...");
            var imageData =
                new ImageUpload
                {
                    // leave zero, if you don't know how height and width is it.
                    Height = 0,
                    Width = 0,
                    Uri = image,
                };

            return instagram.MediaProcessor.UploadPhotoAsync(imageData, caption, location);
        }
    }
}
