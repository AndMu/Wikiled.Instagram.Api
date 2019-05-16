using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaVisualMediaContainerResponse
    {
        [JsonProperty("media")]
        public InstaVisualMediaResponse Media { get; set; }

        [JsonProperty("replay_expiring_at_us")]
        public long? ReplayExpiringAtUs { get; set; }

        [JsonProperty("seen_count")]
        public int? SeenCount { get; set; }

        [JsonProperty("seen_user_ids")]
        public List<long> SeenUserIds { get; set; }

        [JsonProperty("url_expire_at_secs")]
        public long? UrlExpireAtSecs { get; set; }

        [JsonProperty("view_mode")]
        public string ViewMode { get; set; }
    }
}