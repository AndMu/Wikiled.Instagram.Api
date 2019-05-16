using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App.Commands
{
    public class DiscoveryCommand : Command
    {
        private readonly ILogger<DiscoveryCommand> log;

        public DiscoveryCommand(ILogger<DiscoveryCommand> log, DiscoveryConfig config)
            : base(log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        protected override Task Execute(CancellationToken token)
        {
            log.LogInformation("Starting Discovery...");
            
            return Task.CompletedTask;
        }
    }
}
