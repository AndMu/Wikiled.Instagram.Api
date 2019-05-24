using System;
using System.Net.Http;
using Autofac;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Net.Client;
using Wikiled.Common.Net.Resilience;
using Wikiled.Common.Utilities.Modules;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Logic.Builder;
using Wikiled.Instagram.Api.Serialization;
using Wikiled.Instagram.Api.Smart;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Location;
using Wikiled.Instagram.Api.Smart.Tags;
using Wikiled.MachineLearning.Mathematics.Vectors;
using Wikiled.Text.Analysis.Similarity;

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
            builder.RegisterModule<LoggingModule>();
            builder.RegisterType<HttpClient>();
            builder.RegisterInstance(ResilienceConfig.GenerateCommon()).As<IResilienceConfig>();
            builder.RegisterType<CommonResilience>().As<IResilience>();
            builder.RegisterType<GenericClientFactory>().As<IGenericClientFactory>();

            builder.RegisterType<SimilarityDetector>().As<ISimilarityDetector>();
            builder.RegisterType<OneHotEncoder>().As<IWordVectorEncoder>();
            builder.RegisterType<CosineSimilarityDistance>().As<IDistance>();

            builder.RegisterType<CaptionHandler>().As<ICaptionHandler>();
            builder.RegisterType<SimilarMediaTags>().As<ISimilarMediaTags>();
            
            builder.RegisterType<WebSmartTags>().Named<ISmartTags>("Web");
            builder.Register(ctx => new MediaSmartTags(ctx.Resolve<ILogger<MediaSmartTags>>(), ctx.Resolve<ICaptionHandler>(), ctx.ResolveNamed<ISmartTags>("Web"))).As<IMediaSmartTags>();
            builder.RegisterType<InstaSmartTags>().As<ISmartTags>();
            builder.RegisterType<TagEnricher>().As<ITagEnricher>();
            builder.RegisterType<InstaSmartTagsByLocation>().As<ISmartTagsByLocation>();
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
