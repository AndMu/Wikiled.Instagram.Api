using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFullUserInfoUserFeedResponse
    {
        [JsonProperty("auto_load_more_enabled")]
        public bool AutoLoadMoreEnabled { get; set; }

        [JsonProperty("items")] public List<InstaMediaItemResponse> Items { get; set; } = new List<InstaMediaItemResponse>();

        [JsonProperty("more_available")] public bool MoreAvailable { get; set; }

        [JsonProperty("next_max_id")] public string NextMaxId { get; set; }

        [JsonProperty("next_min_id")] public string NextMinId { get; set; }

        [JsonProperty("num_results")] public int NumResults { get; set; }
    }
}
