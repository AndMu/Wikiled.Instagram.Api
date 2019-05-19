using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Smart.Data
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
