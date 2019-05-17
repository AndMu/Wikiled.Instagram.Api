using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class EnrichCommand : Command
    {
        private readonly ILogger<EnrichCommand> log;

        private readonly EnrichConfig config;

        private readonly IInstaApi instagram;

        public EnrichCommand(ILogger<EnrichCommand> log, IInstaApi insta, EnrichConfig config)
            : base(log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            instagram = insta ?? throw new ArgumentNullException(nameof(insta));
        }

        protected override async Task Execute(CancellationToken token)
        {
            log.LogInformation("Enriching Instagram media...");
            instagram.Delay.Disable();
            var logInResult = await instagram.LoginAsync().ConfigureAwait(false);
            instagram.Delay.Enable();
            var currentUser = await instagram.GetCurrentUserAsync().ConfigureAwait(false);
            log.LogInformation("Started for user: {0}...", currentUser.Value.FullName);

            await instagram.UserProcessor.GetUserMediaById(currentUser.Value.Pk, PaginationParameters.Empty)
                .ForEachAsync(
                    item =>
                    {
                        var data = item.Location;
                        log.LogInformation("[{0}] - [{1}] [{2}] with Tags: {3}",
                                           item.Caption.Text,
                                           item.DeviceTimeStamp,
                                           item.Location.ShortName,
                                           item.UserTags.Count);
                        ;
                    }).ConfigureAwait(false);

            log.LogInformation("Completed");
        }
    }
}
