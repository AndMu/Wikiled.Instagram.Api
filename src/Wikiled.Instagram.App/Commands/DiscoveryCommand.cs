using System;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    /// <summary>
    /// Discover new tags
    /// </summary>
    public class DiscoveryCommand : Command
    {
        private readonly ILogger<DiscoveryCommand> log;

        private readonly BasicConfig config;

        private readonly IInstaApi api;

        private readonly ISessionHandler session;

        public DiscoveryCommand(ILogger<DiscoveryCommand> log, IInstaApi api, BasicConfig config, ISessionHandler session)
            : base(log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.session = session ?? throw new ArgumentNullException(nameof(session));
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        protected override async Task Execute(CancellationToken token)
        {
            log.LogInformation("Starting...");
            var location = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var sessionFile = Path.Combine(location, $"{config.User}.session.dat");
            session.Load(sessionFile);
            api.Delay.Disable();
            if (!api.IsUserAuthenticated)
            {
                var logInResult = await api.LoginAsync().ConfigureAwait(false);
            }
            else
            {
                log.LogInformation("User is already authenticated");
            }

            api.Delay.Enable();
            var currentUser = await api.GetCurrentUserAsync().ConfigureAwait(false);
            log.LogInformation("Started for user: {0}...", currentUser.Value.FullName);
            await Internal(currentUser.Value, token).ConfigureAwait(false);
            session.Save(sessionFile);
            log.LogInformation("Completed");
        }

        protected virtual async Task Internal(CurrentUser currentUser, CancellationToken token)
        {
            await api.UserProcessor.GetUserMediaById(currentUser.Pk, PaginationParameters.Empty)
                .ForEachAsync(
                    item =>
                    {
                        var data = item.Location;
                        log.LogInformation("[{0}] - [{1}] [{2}] with Tags: {3}",
                                           item.Caption?.Text,
                                           item.DeviceTimeStamp,
                                           item.Location?.ShortName,
                                           item.UserTags.Count);
                        ;
                    },
                    token)
                .ConfigureAwait(false);
        }
    }
}
