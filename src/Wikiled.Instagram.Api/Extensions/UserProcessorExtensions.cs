using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wikiled.Common.Net.Resilience;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Logic.Processors;

namespace Wikiled.Instagram.Api.Extensions
{
    public static class UserProcessorExtensions
    {
        public static async Task<InstaUser> GetUserSafe(this IUserProcessor processor, string userName, ILogger logger)
        {
            try
            {
                return await processor.Resilience.WebPolicy
                    .ExecuteAsync(async () => await ResultExtension.UnWrap(() => processor.GetUserAsync(userName))
                                      .ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger?.LogDebug(ex.Message);
            }

            return null;
        }
    }
}
