using NLog.Extensions.Logging;
using Wikiled.Common.Logging;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.App.Commands;
using Wikiled.Instagram.App.Commands.Config;

namespace Wikiled.Instagram.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            var starter = new AutoStarter(ApplicationLogging.LoggerFactory, "Instagram Bot", args);
            starter.LoggerFactory.AddNLog();
            starter.RegisterCommand<DiscoveryCommand, DiscoveryConfig>("Discovery");

        }
    }
}
