using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Smart.Data
{
    public class SmartResults
    {
        [JsonProperty("geo")]
        public List<double> Geo { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("results")]
        public List<SmartHashtagResult> Results { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("tagExists")]
        public bool TagExists { get; set; }
    }
}
