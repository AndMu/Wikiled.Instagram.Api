using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFullUserInfoResponse
    {
        [JsonProperty("feed")] public InstaFullUserInfoUserFeedResponse Feed { get; set; }

        [JsonProperty("reel_feed")] public InstaFullUserInfoUserStoryReelResponse ReelFeed { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("user_detail")] public InstaUserInfoContainerResponse UserDetail { get; set; }

        [JsonProperty("user_story")] public InstaFullUserInfoUserStoryResponse UserStory { get; set; }
    }
}
