using NUnit.Framework;
using Wikiled.Instagram.Api.Hashtags.Data;

namespace Wikiled.Instagram.Api.Tests.Hashtags.Data
{
    [TestFixture]
    public class SmartCaptionTests
    {
        [Test]
        public void GenerateExpectedBehavior()
        {
            var instance = new SmartCaption("Message Conto #love");
            instance.WithoutTags = "Message Conto";
            instance.AddTag("Lovex");
            Assert.AreEqual("Message Conto #lovex", instance.Generate());
        }
    }
}
