using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaHashtagStoryResponse
    {
        //[JsonProperty("seen")]
        //public object Seen { get; set; }
        [JsonProperty("can_reply")] public bool CanReply { get; set; }

        [JsonProperty("can_reshare")] public bool CanReshare { get; set; }

        [JsonProperty("expiring_at")] public long ExpiringAt { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("items")] public List<InstaStoryItemResponse> Items { get; set; }

        [JsonProperty("latest_reel_media")] public int LatestReelMedia { get; set; }

        [JsonProperty("muted")] public bool Muted { get; set; }

        [JsonProperty("owner")] public InstaHashtagOwnerResponse Owner { get; set; }

        [JsonProperty("prefetch_count")] public int PrefetchCount { get; set; }

        [JsonProperty("reel_type")] public string ReelType { get; set; }

        [JsonProperty("unique_integer_reel_id")]
        public long UniqueIntegerReelId { get; set; }
    }
}
