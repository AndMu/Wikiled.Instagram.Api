using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Smart.Caption;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public class TagEnricher : ITagEnricher
    {
        private readonly ISmartTagsByLocation tagsByLocation;

        private readonly ISmartTags smartTags;

        private readonly ICaptionHandler captionHandler;

        private readonly ILogger<TagEnricher> logger;

        public TagEnricher(ILogger<TagEnricher> logger, ISmartTagsByLocation tagsByLocation, ISmartTags smartTags, ICaptionHandler captionHandler)
        {
            this.tagsByLocation = tagsByLocation ?? throw new ArgumentNullException(nameof(tagsByLocation));
            this.smartTags = smartTags ?? throw new ArgumentNullException(nameof(smartTags));
            this.captionHandler = captionHandler ?? throw new ArgumentNullException(nameof(captionHandler));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SmartCaption> Enrich(InstaMedia message)
        {
            logger.LogInformation("Generating caption...");
            SmartCaption captionHolder = captionHandler.Extract(message.Caption?.Text);
            logger.LogInformation("Found [{0}] caption tags", captionHolder.TotalTags);

            if (captionHolder.TotalTags > 20)
            {
                logger.LogInformation("Found more than 20 tags on photo - ignoring it");
                return captionHolder;
            }

            var locationTags = await tagsByLocation.Get(message.Location).ConfigureAwait(false);
            foreach (var tag in locationTags.OrderByDescending(item => item.MediaCount).Take(3))
            {
                if (captionHolder.TotalTags >= 27)
                {
                    return captionHolder;
                }

                captionHolder.AddTag(tag);
            }

            var results = await GetMix(captionHolder.Tags.ToArray()).ConfigureAwait(false);
            foreach (var data in results)
            {
                captionHolder.AddTag(data);
            }

            return captionHolder;
        }

        private async Task<HashTagData[]> GetMix(HashTagData[] tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            var table = new Dictionary<string, HashTagData>(StringComparer.OrdinalIgnoreCase);
            foreach (var tag in tags)
            {
                table[tag.Text] = tag;
            }

            var tagsResults = new List<List<HashTagData>>();
            foreach (var tag in tags)
            {
                var result = await smartTags.Get(tag).ConfigureAwait(false);
                result = result.Where(item => item.Rank.HasValue).ToArray();
                if (result.Length > 0)
                {
                    tagsResults.Insert(0, result.OrderBy(item => item.Rank).ToList());
                    tagsResults.Add(result.OrderByDescending(item => item.Relevance).ToList());
                    var relevance = result.Average(item => item.Relevance);
                    tagsResults.Add(result.Where(item => item.Relevance > relevance).OrderByDescending(item => item.Rank).ToList());
                }
            }

            int indexResults = 0;

            while (table.Count < 27)
            {
                if (tagsResults.Count == 0)
                {
                    logger.LogInformation("Tag population completed - all sources depleted");
                    break;
                }

                if (indexResults >= tagsResults.Count)
                {
                    indexResults = 0;
                }

                var current = tagsResults[indexResults];
                bool added = false;
                while (current.Count > 0)
                {
                    var selected = current[0];
                    current.RemoveAt(0);
                    if (!table.ContainsKey(selected.Text))
                    {
                        table.Add(selected.Text, selected);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    tagsResults.RemoveAt(indexResults);
                }
                else
                {
                    indexResults++;
                }
            }

            return table.Values.ToArray();
        }
    }
}
