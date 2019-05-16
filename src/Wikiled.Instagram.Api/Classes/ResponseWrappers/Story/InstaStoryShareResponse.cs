using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryShareResponse
    {
        [JsonProperty("is_linked")]
        public bool IsLinked { get; set; }

        [JsonProperty("is_reel_persisted")]
        public bool IsReelPersisted { get; set; }

        [JsonProperty("media")]
        public InstaMediaItemResponse Media { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("reel_type")]
        public string ReelType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}