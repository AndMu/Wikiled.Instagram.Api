using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverTopLiveResponse
    {
        [JsonProperty("auto_load_more_enabled")]
        public bool AutoLoadMoreEnabled { get; set; }

        [JsonProperty("broadcasts")] public List<InstaBroadcastResponse> Broadcasts { get; set; }

        [JsonProperty("more_available")] public bool MoreAvailable { get; set; }

        [JsonProperty("next_max_id")] public string NextMaxId { get; set; }

        [JsonProperty("post_live_broadcasts")] public List<InstaBroadcastPostLiveResponse> PostLiveBroadcasts { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
