using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wikiled.Geolocation;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Helpers;
using Wikiled.Instagram.Api.Smart.Data;

namespace Wikiled.Instagram.Api.Smart
{
    public class DpSmartTagsByLocation : ISmartTagsByLocation
    {
        private readonly ILogger<DpSmartTagsByLocation> logger;

        public DpSmartTagsByLocation(ILogger<DpSmartTagsByLocation> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HashTagData[]> Get(Location location)
        {
            if (location == null)
            {
                return new HashTagData[] { };
            }

            var radius = new[] { 1, 3, 5, 10, 20, 30, 40, 50, 100 };
            foreach (var i in radius)
            {
                try
                {
                    var result = await GetByLocation(location, i).ConfigureAwait(false);
                    if (result.Count > 0)
                    {
                        return result.Tags.OrderByDescending(item => item.Weight).Select(item => item.Tag)
                                     .Select(HashTagData.FromText)
                                     .ToArray();
                    }

                    await Task.Delay(500).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to retrieve location tags");
                }
            }

            return new HashTagData[] { };
        }

        private async Task<LocationResult> GetByLocation(Location location, int radius)
        {
            var boundaries = new CoordinateBoundaries(location.Lat, location.Lng, radius);

            var client = new HttpClient();
            var query = await client.GetAsync($"https://query.displaypurposes.com/local/?bbox={boundaries.MinLongitude},{boundaries.MinLatitude},{boundaries.MaxLongitude},{boundaries.MaxLatitude}&zoom=10")
                                    .ConfigureAwait(false);
            var text = await query.Content.ReadAsStringAsync().ConfigureAwait(false);
            var results = SerializationHelper.DeserializeFromString<LocationResult>(text);
            return results;
        }
    }
}
