using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightFeedResponse
    {
        [JsonProperty("can_reply")]
        public bool CanReply { get; set; }

        [JsonProperty("can_reshare")]
        public object CanReshare { get; set; }

        [JsonProperty("cover_media")]
        public InstaHighlightCoverMediaResponse CoverMedia { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("latest_reel_media")]
        public int LatestReelMedia { get; set; }

        [JsonProperty("media_count")]
        public int MediaCount { get; set; }

        [JsonProperty("prefetch_count")]
        public int PrefetchCount { get; set; }

        [JsonProperty("ranked_position")]
        public int RankedPosition { get; set; }

        [JsonProperty("reel_type")]
        public string ReelType { get; set; }

        [JsonProperty("seen")]
        public object Seen { get; set; }

        [JsonProperty("seen_ranked_position")]
        public int SeenRankedPosition { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }
    }
}