using Autofac;
using System.ComponentModel.DataAnnotations;
using Wikiled.Console.Arguments;
using Wikiled.Instagram.Api.Modules;

namespace Wikiled.Instagram.App.Commands.Config
{
    public class DiscoveryConfig : ICommandConfig
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Password { get; set; }

        public void Build(ContainerBuilder builder)
        {
            builder.RegisterModule(new InstagramModule(User, Password));
        }
    }
}
