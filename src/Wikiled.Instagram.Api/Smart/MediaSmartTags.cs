using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public class MediaSmartTags : IMediaSmartTags
    {
        private readonly ILogger<MediaSmartTags> log;

        private readonly ICaptionHandler captionHandler;

        private readonly ISmartTags smartTags;

        public MediaSmartTags(ILogger<MediaSmartTags> log, ICaptionHandler captionHandler, ISmartTags smartTags)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.captionHandler = captionHandler ?? throw new ArgumentNullException(nameof(captionHandler));
            this.smartTags = smartTags ?? throw new ArgumentNullException(nameof(smartTags));
        }

        public async Task<HashTagData[]> Get(SectionMedia medias)
        {
            log.LogDebug("Get tags from [{0}] posts", medias.Medias.Count);
            var table = new Dictionary<string, HashTagData>(StringComparer.OrdinalIgnoreCase);
            foreach (InstaMedia media in medias.Medias)
            {
                var text = media.Caption?.Text;
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                var smart = captionHandler.Extract(text);
                foreach (HashTagData tag in smart.Tags.Where(x => !string.IsNullOrEmpty(x.Text)))
                {
                    table[tag.Tag] = tag;
                }
            }

            log.LogInformation("Found [{0}] tags from media. Sending requests....", table.Count);
            var tasks = new List<Task<HashTagData[]>>();
            foreach (var tagData in table)
            {
                tasks.Add(smartTags.Get(tagData.Value));
            }

            foreach (var task in tasks)
            {
                try
                {
                    var result = await task.ConfigureAwait(false);
                    foreach (var tagData in result)
                    {
                        if (table.ContainsKey(tagData.Tag))
                        {
                            table[tagData.Tag] = tagData;
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e, "Processing");
                }
            }

            return table.Values.ToArray();
        }
    }
}
