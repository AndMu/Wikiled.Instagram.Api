using System;
using System.IO;
using Wikiled.Instagram.Api.Helpers;
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
            if (Api == null)
            {
                throw new NullReferenceException("Api is null");
            }

            if (File.Exists(FilePath))
            {
                var data = File.ReadAllText(FilePath);
                var state = SerializationHelper.DeserializeFromString<StateData>(data);
                Api.SetStateData(state);
            }
        }

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        public void Save()
        {
            var state = Api.GetStateData();
            var data = SerializationHelper.SerializeToString(state);
            File.WriteAllText(FilePath, data);
        }
    }
}