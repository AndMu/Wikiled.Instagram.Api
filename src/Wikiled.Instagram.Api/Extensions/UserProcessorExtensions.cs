using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
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
                var result = await ResultExtension
                    .Retry(() => processor.GetUserAsync(userName))
                    .ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return result.Value;
                }
            }
            catch (Exception ex)
            {
                logger?.LogDebug(ex.Message);
            }

            return null;
        }
    }
}
