using System.IO;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Classes.SessionHandlers
{
    public class FileSessionHandler : ISessionHandler
    {
        /// <summary>
        ///     Path to file
        /// </summary>
        public string FilePath { get; set; }

        public IInstaApi Api { get; set; }

        /// <summary>
        ///     Load and Set StateData to InstaApi
        /// </summary>
        public void Load()
        {
            if (File.Exists(FilePath))
            {
                using (var fs = File.OpenRead(FilePath))
                {
                    Api.LoadStateDataFromStream(fs);
                }
            }
        }

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        public void Save()
        {
            using (var state = Api.GetStateDataAsStream())
            {
                using (var fileStream = File.Create(FilePath))
                {
                    state.Seek(0, SeekOrigin.Begin);
                    state.CopyTo(fileStream);
                }
            }
        }
    }
}