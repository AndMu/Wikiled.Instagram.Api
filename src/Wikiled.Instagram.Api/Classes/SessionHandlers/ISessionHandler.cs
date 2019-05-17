using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Classes.SessionHandlers
{
    public interface ISessionHandler
    {
        IInstaApi Api { get; }
        
        /// <summary>
        ///     Load and Set StateData to InstaApi
        /// </summary>
        bool Load(string path);

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        void Save(string path);
    }
}