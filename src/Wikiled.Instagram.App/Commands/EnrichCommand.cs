using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class EnrichCommand : DiscoveryCommand
    {
        private readonly ILogger<EnrichCommand> log;

        private readonly EnrichConfig config;

        private readonly IInstaApi instagram;

        public EnrichCommand(ILogger<EnrichCommand> log,
                             IInstaApi instagram,
                             EnrichConfig config,
                             ISessionHandler session)
            : base(log, instagram, config, session)
        {
            this.log = log;
            this.instagram = instagram;
            this.config = config;
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
                    if (item.DeviceTimeStamp.Date != DateTime.Today)
                    {
                        log.LogInformation("Stop processing old photos");
                        break;
                    }

                    log.LogInformation("Received [{0}] - [{1}] [{2}] with Tags: {3}",
                                       item.Caption?.Text,
                                       item.DeviceTimeStamp,
                                       item.Location?.ShortName,
                                       item.UserTags.Count);


                    for (var index = 0; index < item.Images.Count; index++)
                    {
                        var image = item.Images[index];
                        File.WriteAllBytes($"{item.Identifier}_{index}.jpg", image.ImageBytes);
                    }
                }

                log.LogDebug("Waiting for next cycle");
                await Task.Delay(TimeSpan.FromMinutes(30), token).ConfigureAwait(false);
            } while (!token.IsCancellationRequested);
        }
    }
}
