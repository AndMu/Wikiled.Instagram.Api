using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;

namespace Wikiled.Instagram.Api.Extensions
{
    public static class ResultExtension
    {
        public static async Task<T> UnWrap<T>(this Func<Task<IResult<T>>> action, ILogger logger = null)
        {
            var result = await action().ConfigureAwait(false);
            if (result.Succeeded)
            {
                return result.Value;
            }

            logger?.LogError(result.Info.Message);
            if (result.Info.Exception != null)
            {
                logger?.LogError(result.Info.Exception, "Failed");
                throw result.Info.Exception;
            }

            throw new ApplicationException(result.Info.Message);
        }
    }
}
