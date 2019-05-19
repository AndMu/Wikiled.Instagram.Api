using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Hashtags;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Builder;
using Wikiled.Instagram.Api.Serialization;

namespace Wikiled.Instagram.Api.Modules
{
    public class InstagramModule : Module
    {
        public IRequestDelay Delay { get; set; } = RequestDelay.FromSeconds(2, 2);

        private readonly string user;

        private readonly string password;

        public InstagramModule(string user, string password)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CaptionHandler>().As<ICaptionHandler>();
            builder.RegisterType<SmartTagsManager>().As<ISmartTagsManager>();
            builder.RegisterType<PlainSerializer>().Named<ISerializer>("Plain");
            builder.Register(ctx => new EncryptedSerializer(ctx.ResolveNamed<ISerializer>("Plain"), ctx.Resolve<IInstaApi>()))
                .As<ISerializer>();
            builder.RegisterType<FileSessionHandler>().As<ISessionHandler>();
            builder.RegisterInstance(new UserSessionData { UserName = user, Password = password });
            builder.Register(
                ctx =>
                    InstaApiBuilder.CreateBuilder()
                                   .SetUser(ctx.Resolve<UserSessionData>())
                                   .UseLogger(ctx.Resolve<ILoggerFactory>())
                                   .SetRequestDelay(Delay)
                                   .Build())
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
