using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Extensions;
using Wikiled.Common.Utilities.Serialization;

namespace Wikiled.Instagram.Api.Smart.Helpers
{
    public class FileCachedCall : ICachedCall
    {
        private readonly ILogger<FileCachedCall> logger;

        private readonly string path;

        public FileCachedCall(ILogger<FileCachedCall> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location), "Tags");
            path.EnsureDirectoryExistence();
        }

        public async Task<TResult> Get<TArg, TResult>(TArg arg, Func<TArg, Task<TResult>> underlying, Func<TArg, string> name)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            if (underlying == null)
            {
                throw new ArgumentNullException(nameof(underlying));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var file = Path.Combine(path, $"{name(arg)}.zip");
            if (File.Exists(file))
            {
                logger.LogInformation("Loading <{0}>", file);
                return await JsonSerializationExtension.DeserializeJsonZip<TResult>(file).ConfigureAwait(false);
            }

            var result = await underlying(arg).ConfigureAwait(false);
            logger.LogInformation("Saving <{0}>", file);
            await result.SerializeJsonZip(file).ConfigureAwait(false);
            return result;
        }
    }
}
