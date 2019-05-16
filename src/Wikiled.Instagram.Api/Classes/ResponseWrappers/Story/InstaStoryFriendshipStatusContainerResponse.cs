using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryFriendshipStatusContainerResponse
    {
        [JsonProperty("friendship_status")] public InstaStoryFriendshipStatusResponse FriendshipStatus { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
