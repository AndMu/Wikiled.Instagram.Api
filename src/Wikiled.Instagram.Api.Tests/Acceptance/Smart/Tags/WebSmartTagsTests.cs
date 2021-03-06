﻿using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Wikiled.Instagram.Api.Smart.Data;
using Wikiled.Instagram.Api.Smart.Tags;

namespace Wikiled.Instagram.Api.Tests.Acceptance.Smart.Tags
{
    [TestFixture]
    public class WebSmartTagsTests
    {
        private ISmartTags instance;

        [SetUp]
        public void Setup()
        {
            instance = Global.Container.ResolveNamed<ISmartTags>("Web");
        }

        [Test]
        public async Task GetSmart()
        {
            var result = await instance.Get(HashTagData.FromText("london")).ConfigureAwait(false);
            Assert.Greater(result.Length, 20);
        }
    }
}
