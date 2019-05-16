using System.Threading.Tasks;

namespace Wikiled.Instagram.Api.Logic.Processors
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