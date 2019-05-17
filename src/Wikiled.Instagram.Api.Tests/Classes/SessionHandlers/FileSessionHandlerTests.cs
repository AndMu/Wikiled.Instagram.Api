using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.SessionHandlers;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Serialization;

namespace Wikiled.Instagram.Api.Tests.Classes.SessionHandlers
{
    [TestFixture]
    public class FileSessionHandlerTests
    {
        private Mock<IInstaApi> mockInstaApi;

        private FileSessionHandler instance;

        private StateData data;

        private string defaultPath;

        [SetUp]
        public void SetUp()
        {
            defaultPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "data", "sample.session.dat");
            mockInstaApi = new Mock<IInstaApi>();
            mockInstaApi.Setup(item => item.SetStateData(It.IsAny<StateData>()))
                .Callback<StateData>(state => { data = state;});
            instance = CreateInstance();
        }

        [Test]
        public void Load()
        {
            instance.Load(defaultPath);
            Assert.IsNotNull(data);
            Assert.AreEqual("xxxx", data.UserSession.UserName);
        }

        [Test]
        public void SaveEncrypted()
        {
            instance.Load(defaultPath);
            var newFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "new.session.dat");
            data.UserSession.Password = "Test1";
            mockInstaApi.Setup(item => item.GetStateData()).Returns(data);
            instance = new FileSessionHandler(new NullLogger<FileSessionHandler>(),
                                              mockInstaApi.Object,
                                              new EncryptedSerializer(new PlainSerializer(), mockInstaApi.Object));
            instance.Save(newFile);
            data = null;
            var result = instance.Load(newFile);
            Assert.IsTrue(result);
            Assert.IsNotNull(data);
            Assert.AreEqual("Test1", data.UserSession.Password);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new FileSessionHandler(new NullLogger<FileSessionHandler>(), null, new PlainSerializer()));
            Assert.Throws<ArgumentNullException>(() => new FileSessionHandler(null, mockInstaApi.Object, new PlainSerializer()));
            Assert.Throws<ArgumentNullException>(() => new FileSessionHandler(new NullLogger<FileSessionHandler>(), mockInstaApi.Object, null));
        }

        private FileSessionHandler CreateInstance()
        {
            return new FileSessionHandler(new NullLogger<FileSessionHandler>(), mockInstaApi.Object, new PlainSerializer());
        }
    }
}
