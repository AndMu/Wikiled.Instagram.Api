using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaReelFeedResponse
    {
        [JsonProperty("can_reply")] public bool CanReply { get; set; }

        [JsonProperty("can_reshare")] public string CanReshare { get; set; }

        [JsonProperty("expiring_at")] public long ExpiringAt { get; set; }

        [JsonProperty("has_besties_media")] public long HasBestiesMedia { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("items")] public List<InstaStoryItemResponse> Items { get; set; }

        [JsonProperty("latest_reel_media")] public long? LatestReelMedia { get; set; }

        [JsonProperty("prefetch_count")] public long PrefetchCount { get; set; }

        [JsonProperty("seen")] public long? Seen { get; set; }

        [JsonProperty("user")] public InstaUserShortFriendshipFullResponse User { get; set; }
    }
}
