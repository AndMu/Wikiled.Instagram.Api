using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTvSearchResponse
    {
        [JsonProperty("num_results")]
        public int? NumResults { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; }

        [JsonProperty("results")]
        public List<InstaTvSearchResultResponse> Results { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}