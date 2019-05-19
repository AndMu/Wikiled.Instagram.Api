using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Smart;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    //Memories from Barcelona 🥰 #barcelona🇪🇸 #Spain #weekendgetaway #trip #travel
    public class EnrichCommand : DiscoveryCommand
    {
        private readonly ILogger<EnrichCommand> log;

        private readonly EnrichConfig config;

        private readonly IInstaApi instagram;

        private readonly ITagEnricher tagsManager;

        private readonly string location;

        private readonly ConcurrentDictionary<string, string> processed = new ConcurrentDictionary<string, string>();

        public EnrichCommand(ILogger<EnrichCommand> log, IInstaApi instagram, EnrichConfig config, ISessionHandler session, ITagEnricher tagsManager)
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
        }

        protected override async Task Internal(CurrentUser currentUser, CancellationToken token)
        {
            do
            {
                InstaMedia[] media = await instagram.UserProcessor
                    .GetUserMediaById(currentUser.Pk, PaginationParameters.MaxPagesToLoad(1))
                    .ToArray();
                foreach (InstaMedia item in media)
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

            InstaImage image = item.Images.OrderByDescending(x => x.Height).FirstOrDefault();
            if (image == null)
            {
                log.LogWarning("Image not found");
                return;
            }

            var caption = await tagsManager.Enrich(item).ConfigureAwait(false);
            var captionText = caption.Generate();
            if (captionText == caption.Original)
            {
                log.LogInformation("Caption is same as original");
                return;
            }

            var fileName = await DownloadImage(item, image).ConfigureAwait(false);
            await instagram.MediaProcessor.DeleteMediaAsync(item.Identifier, item.MediaType).ConfigureAwait(false);
            await UploadImage(fileName, captionText, item.Location).ConfigureAwait(false);
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
