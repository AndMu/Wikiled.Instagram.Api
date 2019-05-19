using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class LocationTagResult
    {
        [JsonProperty("centroid")]
        public List<double> Centroid { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}
