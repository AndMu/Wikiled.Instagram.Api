using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
{
    public class FilePersistedSmartTags : ISmartTags
    {
        private readonly ILogger<FilePersistedSmartTags> logger;

        private string path;

        public FilePersistedSmartTags(ILogger<FilePersistedSmartTags> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        }

        public Task<HashTagData[]> Get(HashTagData tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var file = Path.Combine(path, tag.Text);
            if ()
            throw new System.NotImplementedException();
        }
    }
}
