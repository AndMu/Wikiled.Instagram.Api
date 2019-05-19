using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Smart;

namespace Wikiled.Instagram.Api.Tests.Smart
{
    [TestFixture]
    public class DpSmartTagsByLocationTests
    {
        private DpSmartTagsByLocation instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateDpSmartTagsByLocation();
        }
       
        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new DpSmartTagsByLocation(null));
        }

        [Test]
        public async Task GetByLocationSmart()
        {
            var result = await instance.Get(new Location { Lat = 51.37457394, Lng = 0.52192198333333 }).ConfigureAwait(false);
            Assert.AreEqual(2, result.Length);
        }

        private DpSmartTagsByLocation CreateDpSmartTagsByLocation()
        {
            return new DpSmartTagsByLocation(new NullLogger<DpSmartTagsByLocation>());
        }
    }
}
