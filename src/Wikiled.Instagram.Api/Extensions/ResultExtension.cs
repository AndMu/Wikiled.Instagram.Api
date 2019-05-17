using System;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes;

namespace Wikiled.Instagram.Api.Extensions
{
    public static class ResultExtension
    {
        public static async Task<IResult<T>> Retry<T>(this Func<Task<IResult<T>>> action, int time = 2)
        {
            IResult<T> result = null;
            for (int i = 0; i < time; i++)
            {
                result = await action().ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
