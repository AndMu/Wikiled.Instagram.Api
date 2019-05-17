using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Extensions;

namespace Wikiled.Instagram.Api.Tests.Extensions
{
    [TestFixture]
    public class ResultExtensionTests
    {
        [TestCase(false, 2)]
        [TestCase(true, 1)]
        public async Task Retry(bool succeeded, int times)
        {
            int total = 0;
            var task = (Func<Task<IResult<InstaUser>>>)(() =>
            {
                total++;
                return Task.FromResult((IResult<InstaUser>)new InstaResult<InstaUser>(succeeded, (InstaUser)null));
            });

            await task.Retry().ConfigureAwait(false);
            Assert.AreEqual(times, total);
        }
    }
}
