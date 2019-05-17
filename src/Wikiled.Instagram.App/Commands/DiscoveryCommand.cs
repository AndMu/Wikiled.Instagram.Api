using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class DiscoveryCommand : Command
    {
        private readonly ILogger<DiscoveryCommand> log;

        private readonly DiscoveryConfig config;

        private IInstaApi instagram;

        public DiscoveryCommand(ILogger<DiscoveryCommand> log, IInstaApi insta, DiscoveryConfig config)
            : base(log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.instagram = insta ?? throw new ArgumentNullException(nameof(insta));
        }

        protected override async Task Execute(CancellationToken token)
        {
            log.LogInformation("Starting Discovery...");
            instagram.Delay.Disable();
            var logInResult = await instagram.LoginAsync().ConfigureAwait(false);
            instagram.Delay.Enable();
            var currentUser = await instagram.GetCurrentUserAsync();
            log.LogInformation("Started for user: {0}...", currentUser.Value.FullName);
        }
    }
}
