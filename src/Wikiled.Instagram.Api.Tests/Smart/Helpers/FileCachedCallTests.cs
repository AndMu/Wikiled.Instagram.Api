using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Smart.Data;
using Wikiled.Instagram.Api.Smart.Helpers;

namespace Wikiled.Instagram.Api.Tests.Smart.Tags
{
    [TestFixture]
    public class FileCachedCallTests
    {
        private FileCachedCall instance;

        private SmartResults smartResult;

        [SetUp]
        public void SetUp()
        {
            instance = CreateInstance();
            smartResult = new SmartResults();
            smartResult.Tag = "Test";
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new FileCachedCall(null));
        }

        [Test]
        public async Task Get()
        {
            var key = Guid.NewGuid().ToString();
            var result = await instance.Get(key, arg => Task.FromResult(smartResult), args => args).ConfigureAwait(false);
            Assert.AreEqual("Test", result.Tag);
        }

        [Test]
        public async Task GetSaved()
        {
            var key = Guid.NewGuid().ToString();
            var result = await instance.Get(key, arg => Task.FromResult(smartResult), args => args).ConfigureAwait(false);
            var result2 = await instance.Get(key, arg => Task.FromResult(smartResult), args => args).ConfigureAwait(false);
            Assert.AreNotSame(result2, result);
            Assert.AreEqual("Test", result2.Tag);
        }

        private FileCachedCall CreateInstance()
        {
            return new FileCachedCall(new NullLogger<FileCachedCall>());
        }
    }
}
