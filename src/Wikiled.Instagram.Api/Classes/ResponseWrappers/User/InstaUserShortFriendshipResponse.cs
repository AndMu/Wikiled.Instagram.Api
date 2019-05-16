using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserShortFriendshipResponse : InstaUserShortResponse
    {
        [JsonProperty("friendship_status")] public InstaFriendshipShortStatusResponse FriendshipStatus { get; set; }
    }
}
