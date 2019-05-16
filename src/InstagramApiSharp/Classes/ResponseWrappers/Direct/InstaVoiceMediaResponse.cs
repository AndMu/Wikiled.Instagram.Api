using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaVoiceMediaResponse
    {
        [JsonProperty("media")] public InstaVoiceResponse Media { get; set; }

        [JsonProperty("replay_expiring_at_us")]
        public string ReplayExpiringAtUs { get; set; }

        [JsonProperty("seen_count")] public int? SeenCount { get; set; }

        [JsonProperty("seen_user_ids")] public long[] SeenUserIds { get; set; }

        [JsonProperty("view_mode")] public string ViewMode { get; set; }
    }
}
