using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserShortFriendshipFullResponse : InstaUserShortResponse
    {
        [JsonProperty("friendship_status")]
        public InstaFriendshipFullStatusResponse FriendshipStatus { get; set; }
    }
}