using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Smart.Data
{
    public class LocationResult
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("tags")]
        public List<LocationTagResult> Tags { get; set; }
    }
}
