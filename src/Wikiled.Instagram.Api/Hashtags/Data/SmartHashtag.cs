using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Hashtags.Data
{
    public class SmartHashtag
    {
        [JsonProperty("absRelevance")]
        public double AbsRelevance { get; set; }

        [JsonProperty("geo")]
        public List<double?> Geo { get; set; }

        [JsonProperty("media_count")]
        public int MediaCount { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("relevance")]
        public int Relevance { get; set; }

        [JsonProperty("tag")] 
        public string Tag { get; set; }
    }
}
