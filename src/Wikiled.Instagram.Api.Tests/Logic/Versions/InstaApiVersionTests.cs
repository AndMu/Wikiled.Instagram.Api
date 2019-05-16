using System;
using Moq;
using NUnit.Framework;
using Wikiled.Instagram.Api.Logic.Versions;

namespace Wikiled.Instagram.Api.Tests.Logic.Versions
{
    [TestFixture]
    public class InstaApiVersionTests
    {
        private InstaApiVersion instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateInstaApiVersion();
        }

        [Test]
        public void Construct()
        {
            Assert.IsNotNull(instance);
        }

        private InstaApiVersion CreateInstaApiVersion()
        {
            return new InstaApiVersion();
        }
    }
}
