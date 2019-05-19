using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Wikiled.Instagram.Api.Smart;

namespace Wikiled.Instagram.Api.Tests.Smart
{
    [TestFixture]
    public class CaptionHandlerTests
    {
        private CaptionHandler instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateCaptionHandler();
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new CaptionHandler(null));
        }

        [Test]
        public void Extract()
        {
            var result = instance.Extract("My tags are here #tags #london");
            Assert.AreEqual("My tags are here #tags #london", result.Original);
            Assert.AreEqual("My tags are here", result.WithoutTags);
            Assert.AreEqual(2, result.TotalTags);
            var lookup = result.Tags.ToLookup(item => item.Text);
            Assert.IsTrue(lookup.Contains("tags"));
            Assert.IsTrue(lookup.Contains("london"));
        }

//        Beyond beautiful!❤️
//        📸: @dorpell
//        🔺Admin: @karlvibes
//            Location: Barcelona, Spain 🇪🇸 Tag your best pictures with #city_delight
//        •
//        •
//        •
//        •
//        •
//#theprettycities#discover_europe_#europe_perfection#awesome_earthpix#cbviews#bestcitybreaks#hello_worldpics#perfect_worldplaces#ig_europe#besteuropephotos#suitcasetravels#topeuropephoto#bestplacestogo#wonderful_places#stayandwander#seemycity#travelworld_addiction#travel_drops#mybestcityshots#alluring_villages#kings_villages#awesome_phototrip#villages#citiesoftheworld#spaintravel#spain_vacations#barcelonaspain#barcelona#barcelona🇪🇸

        private CaptionHandler CreateCaptionHandler()
        {
            return new CaptionHandler(new NullLogger<CaptionHandler>());
        }
    }
}