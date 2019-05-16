using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaRecentActivityStoryItemResponse
    {
        [JsonProperty("inline_follow")]
        public InstaInlineFollowResponse InlineFollow { get; set; }

        [JsonProperty("links")]
        public List<InstaLinkResponse> Links { get; set; }

        [JsonProperty("media")]
        public List<InstaActivityMediaResponse> Media { get; set; }

        [JsonProperty("profile_id")]
        public long ProfileId { get; set; }

        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }

        [JsonProperty("rich_text")]
        public string RichText { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }
    }
}