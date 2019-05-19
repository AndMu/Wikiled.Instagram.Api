using NUnit.Framework;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Tests.Smart.Data
{
    [TestFixture]
    public class SmartCaptionTests
    {
        [Test]
        public void GenerateExpectedBehavior()
        {
            var instance = new SmartCaption("Message Conto #love");
            instance.WithoutTags = "Message Conto";
            instance.AddTag(HashTagData.FromText("Lovex"));
            Assert.AreEqual("Message Conto #lovex", instance.Generate());
        }
    }
}
