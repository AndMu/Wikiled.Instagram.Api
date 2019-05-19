using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class LocationResults
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("tags")]
        public List<LocationTag> Tags { get; set; }
    }
}
