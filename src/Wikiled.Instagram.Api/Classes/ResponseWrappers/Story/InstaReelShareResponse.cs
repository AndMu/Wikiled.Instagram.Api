using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaReelShareResponse
    {
        [JsonProperty("is_reel_persisted")]
        public bool? IsReelPersisted { get; set; }

        [JsonProperty("media")]
        public InstaStoryItemResponse Media { get; set; }

        [JsonProperty("reel_owner_id")]
        public long ReelOwnerId { get; set; }

        [JsonProperty("reel_type")]
        public string ReelType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}