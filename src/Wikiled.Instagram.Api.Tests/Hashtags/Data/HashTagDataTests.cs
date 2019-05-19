using System;
using NUnit.Framework;
using Wikiled.Instagram.Api.Hashtags.Data;

namespace Wikiled.Instagram.Api.Tests.Hashtags.Data
{
    [TestFixture]
    public class HashTagDataTests
    {
        [TestCase("#tag", "tag", "#tag")]
        [TestCase("#TAG", "tag", "#tag")]
        public void FromTag(string original, string text, string tag)
        {
            var created = HashTagData.FromTag(original);
            Assert.AreEqual(tag, created.Tag);
            Assert.AreEqual(text, created.Text);
        }

        [Test]
        public void FromTagArgumnets()
        {
           Assert.Throws<ArgumentNullException>(() => HashTagData.FromTag(null));
           Assert.Throws<ArgumentOutOfRangeException>(() => HashTagData.FromTag("text"));
        }

        [TestCase("tag", "tag", "#tag")]
        [TestCase("TAG", "tag", "#tag")]
        public void FromText(string original, string text, string tag)
        {
            var created = HashTagData.FromText(original);
            Assert.AreEqual(tag, created.Tag);
            Assert.AreEqual(text, created.Text);
        }

        [Test]
        public void FromTextArgumnets()
        {
            Assert.Throws<ArgumentNullException>(() => HashTagData.FromText(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => HashTagData.FromText("#text"));
        }
    }
}