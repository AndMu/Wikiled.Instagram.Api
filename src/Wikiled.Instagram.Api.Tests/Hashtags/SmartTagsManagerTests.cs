using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Hashtags;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Tests.Hashtags
{
    [TestFixture]
    public class SmartTagsManagerTests
    {
        private SmartTagsManager instance;

        private Mock<IInstaApi> api;

        [SetUp]
        public void SetUp()
        {
            api = new Mock<IInstaApi>();
            instance = CreateManager();
        }

        [Test]
        public async Task GetByLocation()
        {
            var result = await instance.GetByLocation(new Location { Lat = 51.37457394, Lng = 0.52192198333333 }, 50).ConfigureAwait(false);
            Assert.AreEqual(187, result.Tags.Count);
        }

        [Test]
        public async Task GetByLocationSmart()
        {
            var result = await instance.GetByLocationSmart(new Location { Lat = 51.37457394, Lng = 0.52192198333333 }).ConfigureAwait(false);
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public async Task GetAll()
        {
            var result = await instance.GetAll("london", "fashion").ConfigureAwait(false);
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public async Task GetSmart()
        {
            var result = await instance.GetSmart(27, "london", "fashion").ConfigureAwait(false);
            Assert.AreEqual(27, result.Length);
        }

        private SmartTagsManager CreateManager()
        {
            return new SmartTagsManager(new NullLogger<SmartTagsManager>(), api.Object);
        }
    }
}
