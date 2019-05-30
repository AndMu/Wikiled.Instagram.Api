using System;
using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Smart.Helpers
{
    public interface ICachedCall
    {
        Task<TResult> Get<TArg, TResult>(TArg arg, Func<TArg, Task<TResult>> underlying, Func<TArg, string> name);
    }
}