using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Wikiled.Geolocation;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Hashtags.Data;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Logic;

namespace Wikiled.Instagram.Api.Hashtags
{
    public class SmartTagsManager : ISmartTagsManager
    {
        private readonly ILogger<SmartTagsManager> logger;

        private IInstaApi api;

        public SmartTagsManager(ILogger<SmartTagsManager> logger, IInstaApi api)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public async Task<string[]> GetByLocationSmart(Location location, int total = 3)
        {
            if (location == null)
            {
                return new string[] { };
            }

            var radius = new[] {1, 3, 5, 10, 20, 30, 40, 50, 100};
            foreach (var i in radius)
            {
                try
                {
                    var result = await GetByLocation(location, i).ConfigureAwait(false);
                    if (result.Count > 0)
                    {
                        return result.Tags.OrderByDescending(item => item.Weight).Select(item => item.Tag).Take(total).ToArray();
                    }

                    await Task.Delay(500).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to retrieve location tags");
                }
            }

            return new string[] { };
        }

        public async Task<LocationResult> GetByLocation(Location location, int radius)
        {
            var boundaries = new CoordinateBoundaries(location.Lat, location.Lng, radius);

            var client = new HttpClient();
            var query = await client.GetAsync($"https://query.displaypurposes.com/local/?bbox={boundaries.MinLongitude},{boundaries.MinLatitude},{boundaries.MaxLongitude},{boundaries.MaxLatitude}&zoom=10")
                                    .ConfigureAwait(false);
            var text = await query.Content.ReadAsStringAsync().ConfigureAwait(false);
            var results = SerializationHelper.DeserializeFromString<LocationResult>(text);
            return results;
        }

        public async Task<string[]> GetSmart(int total, params string[] tags)
        {
            if (total < tags.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(total));
            }

            var retrieved = await GetAll(tags).ConfigureAwait(false);
            var tagsResults = new List<List<SmartHashtagResult>>();
            for (var i = 0; i < retrieved.Length; i++)
            {
                tagsResults.Add(retrieved[i].Results.OrderBy(item => item.Rank).ToList());
            }

            for (var i = 0; i < retrieved.Length; i++)
            {
                if (retrieved[i].Results.Count > 0)
                {
                    tagsResults.Add(retrieved[i].Results.OrderByDescending(item => item.Relevance).ToList());
                }
            }

            for (var i = 0; i < retrieved.Length; i++)
            {
                var relevance = retrieved[i].Results.Average(item => item.Relevance);
                tagsResults.Add(retrieved[i].Results.Where(item => item.Relevance > relevance).OrderByDescending(item => item.Rank).ToList());
            }

            var result = new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase);
            int indexResults = 0;

            while (result.Count < total)
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
                for (int j = 0; j < current.Count; j++)
                {
                    var selected = current[j];
                    if (!result.Contains(selected.Tag))
                    {
                        result.Add(selected.Tag);
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

            return result.ToArray();
        }

        public async Task<SmartResults[]> GetAll(params string[] tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            var results = new List<Task<HttpResponseMessage>>();
            foreach (var tag in tags)
            {
                var client = new HttpClient();
                var query = client.GetAsync($"https://d212rkvo8t62el.cloudfront.net/tag/{tag}");
                results.Add(query);
            }

            var finalResults = new List<SmartResults>();
            foreach (var result in results)
            {
                try
                {
                    var responce = await result.ConfigureAwait(false);
                    var text = await responce.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var tag = SerializationHelper.DeserializeFromString<SmartResults>(text);
                    finalResults.Add(tag);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Processing");
                }
            }

            return finalResults.Where(item => item.Results.Count > 0).ToArray();
        }
    }
}
