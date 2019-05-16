using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.API.Processors
{
    public interface IPushProcessor
    {
        /// <summary>
        ///     Registers application for push notifications
        /// </summary>
        /// <returns></returns>
        Task<bool> RegisterPush();
    }
}
