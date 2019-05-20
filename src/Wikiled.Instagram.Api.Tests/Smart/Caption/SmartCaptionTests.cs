using NUnit.Framework;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Tests.Smart.Caption
{
    [TestFixture]
    public class SmartCaptionTests
    {
        [Test]
        public void GenerateExpectedBehavior()
        {
            var instance = new SmartCaption("Message Conto #love", new[] { HashTagData.FromTag("#love") });
            instance.AddTag(HashTagData.FromText("Love"));
            Assert.AreEqual("Message Conto #love", instance.Generate());
            instance.AddTag(HashTagData.FromText("Lovex"));
            Assert.AreEqual("Message Conto #love #lovex", instance.Generate());
        }
    }
}
