using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class LocationResult
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("tags")]
        public List<LocationTagResult> Tags { get; set; }
    }
}
