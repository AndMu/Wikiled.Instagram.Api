using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Wikiled.Common.Net.Client;
using Wikiled.Instagram.Api.Smart.Data;
using Wikiled.Instagram.Api.Smart.Tags;

namespace Wikiled.Instagram.Api.Tests.Smart.Tags
{
    [TestFixture]
    public class WebSmartTagsTests
    {
        private WebSmartTags instance;

        private Mock<IGenericClientFactory> clientFactory;

        private Mock<IResilientApiClient> client;

        [SetUp]
        public void SetUp()
        {
            client = new Mock<IResilientApiClient>();
            clientFactory = new Mock<IGenericClientFactory>();
            clientFactory.Setup(item => item.ConstructResilient(It.IsAny<Uri>())).Returns(client.Object);
            instance = CreateManager();
        }

        [Test]
        public void Constructor()
        {
            Assert.Throws<ArgumentNullException>(
                () => new WebSmartTags(null, clientFactory.Object));
            Assert.Throws<ArgumentNullException>(
                () => new WebSmartTags(new NullLogger<WebSmartTags>(), null));
        }

        [Test]
        public async Task GetSmart()
        {
            client.Setup(item => item.GetRequest<SmartResults>("london", CancellationToken.None))
                .Returns(Task.FromResult(new SmartResults
                {
                    Results = new List<SmartHashtagResult>(
                        new[] { new SmartHashtagResult { Tag = "Test" } })
                }));
            var result = await instance.Get(HashTagData.FromText("london")).ConfigureAwait(false);
            Assert.AreEqual(1, result.Length);
        }

        private WebSmartTags CreateManager()
        {
            return new WebSmartTags(new NullLogger<WebSmartTags>(), clientFactory.Object);
        }
    }
}
