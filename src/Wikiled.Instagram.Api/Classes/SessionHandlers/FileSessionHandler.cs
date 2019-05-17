using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Wikiled.Instagram.Api.Logic;
using Wikiled.Instagram.Api.Serialization;

namespace Wikiled.Instagram.Api.Classes.SessionHandlers
{
    public class FileSessionHandler : ISessionHandler
    {
        private readonly ILogger<FileSessionHandler> logger;

        private readonly ISerializer serializer;

        public FileSessionHandler(ILogger<FileSessionHandler> logger, IInstaApi api, ISerializer serializer)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IInstaApi Api { get; }

        /// <summary>
        ///     Load and Set StateData to InstaApi
        /// </summary>
        public bool Load(string path)
        {
            logger.LogInformation("Load <{0}>", path);
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (Api == null)
            {
                throw new NullReferenceException("Api is null");
            }

            try
            {
                if (File.Exists(path))
                {
                    var data = File.ReadAllText(path);
                    var state = serializer.DeSerialize<StateData>(data);
                    Api.SetStateData(state);
                    return true;
                }

                logger.LogInformation("File not found: <{0}>", path);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Loading");
            }

            return false;
        }

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        public void Save(string path)
        {
            logger.LogInformation("Save <{0}>", path);
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var state = Api.GetStateData();
            var data = serializer.Serialize(state);
            File.WriteAllText(path, data);
        }
    }
}