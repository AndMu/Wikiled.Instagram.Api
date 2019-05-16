using Autofac;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.Api.Modules;

namespace Wikiled.Instagram.App.Commands.Config
{
    public class DiscoveryConfig : ICommandConfig
    {
        public void Build(ContainerBuilder builder)
        {
            builder.RegisterModule<InstagramModule>();
        }
    }
}
