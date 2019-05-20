using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Wikiled.Instagram.Api.Smart.Caption;

namespace Wikiled.Instagram.Api.Tests.Smart.Caption
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
            Assert.AreEqual(2, result.TotalTags);
            var lookup = result.Tags.ToLookup(item => item.Text);
            Assert.IsTrue(lookup.Contains("tags"));
            Assert.IsTrue(lookup.Contains("london"));
        }

        [TestCase("#Tag_one#Tag_two", 2)]
        [TestCase("#Tag_one #Tag_two", 2)]
        [TestCase("#Tag_one#Кремль", 2)]
        public void ExtractCases(string caption, int total)
        {
            var result = instance.Extract(caption);
            Assert.AreEqual(total, result.Tags.Count());
        }

        [Test]
        public void ExtractComlex()
        {
            var result = instance.Extract("Beyond beautiful!❤️\r\n" +
                                          "📸: @dorpell\r\n" +
                                          "🔺Admin: @karlvibes\r\n" +
                                          "Location: Barcelona, Spain 🇪🇸 Tag your best pictures with #city_delight\r\n" +
                                          "•\r\n" +
                                          "•\r\n" +
                                          "•\r\n" +
                                          "•\r\n" +
                                          "•\r\n" +
                                          "#theprettycities#discover_europe_#europe_perfection#awesome_earthpix#cbviews#bestcitybreaks#hello_worldpics#perfect_worldplaces#ig_europe#besteuropephotos#suitcasetravels#topeuropephoto#bestplacestogo#wonderful_places#stayandwander#seemycity#travelworld_addiction#travel_drops#mybestcityshots#alluring_villages#kings_villages#awesome_phototrip#villages#citiesoftheworld#spaintravel#spain_vacations#barcelonaspain#barcelona#barcelona🇪🇸");
            Assert.AreEqual(29, result.TotalTags);
        }

        private CaptionHandler CreateCaptionHandler()
        {
            return new CaptionHandler(new NullLogger<CaptionHandler>());
        }
    }
}