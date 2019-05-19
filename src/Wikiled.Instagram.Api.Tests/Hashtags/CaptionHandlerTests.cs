using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework.Constraints;
using Wikiled.Instagram.Api.Hashtags;

namespace Wikiled.Instagram.Api.Tests.Hashtags
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
        public void ExtractExpectedBehavior()
        {
            var result = instance.Extract("My tags are here #tags #london");
            Assert.AreEqual("My tags are here #tags #london", result.Original);
            Assert.AreEqual("My tags are here", result.WithoutTags);
            Assert.AreEqual(2, result.Tags.Count());
            var lookup = result.Tags.ToLookup(item => item.Text);
            Assert.IsTrue(lookup.Contains("tags"));
            Assert.IsTrue(lookup.Contains("london"));
        }

        private CaptionHandler CreateCaptionHandler()
        {
            return new CaptionHandler(new NullLogger<CaptionHandler>());
        }
    }
}