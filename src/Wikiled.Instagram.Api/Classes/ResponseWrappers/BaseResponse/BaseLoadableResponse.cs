using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse
{
    public class InstaBaseLoadableResponse : InstaBaseStatusResponse
    {
        [JsonProperty("auto_load_more_enabled")]
        public bool AutoLoadMoreEnabled { get; set; }

        [JsonProperty("more_available")]
        public bool MoreAvailable { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; } = "unknown";

        [JsonProperty("num_results")]
        public int ResultsCount { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}