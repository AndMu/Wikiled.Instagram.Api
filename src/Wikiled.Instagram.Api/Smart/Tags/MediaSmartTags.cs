using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart.Tags
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
                    if (!table.ContainsKey(tag.Tag))
                    {
                        table[tag.Tag] = tag;
                        tag.MediaCount = 0;
                    }

                    table[tag.Tag].MediaCount += 1;
                }
            }

            log.LogInformation("Enriching {0} tags", table.Count);
            foreach (var data in table.ToArray())
            {
                if (table[data.Key].Rank.HasValue)
                {
                    continue;
                }

                var result = await smartTags.Get(data.Value).ConfigureAwait(false);
                foreach (var hashTagData in result)
                {
                    if (table.ContainsKey(hashTagData.Tag))
                    {
                        table[hashTagData.Tag] = hashTagData;
                    }
                }
            }

            return table.Values.ToArray();
        }
    }
}
