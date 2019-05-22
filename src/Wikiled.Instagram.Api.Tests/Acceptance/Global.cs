using Autofac;
using NUnit.Framework;
using Wikiled.Instagram.Api.Modules;

namespace Wikiled.Instagram.Api.Tests.Acceptance
{
    [SetUpFixture]
    public class Global
    {
        public static IContainer  Container { get; private set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InstagramModule("Test", "Test"));
            Container = builder.Build();
        }
    }
}
