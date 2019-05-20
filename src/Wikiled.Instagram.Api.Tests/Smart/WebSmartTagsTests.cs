using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Wikiled.Instagram.Api.Smart;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Tests.Smart
{
    [TestFixture]
    public class WebSmartTagsTests
    {
        private WebSmartTags instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateManager();
        }

        [Test]
        public async Task GetSmart()
        {
            var result = await instance.Get(HashTagData.FromText("london")).ConfigureAwait(false);
            Assert.Greater(result.Length, 20);
        }

        private WebSmartTags CreateManager()
        {
            return new WebSmartTags(new NullLogger<WebSmartTags>());
        }
    }
}
