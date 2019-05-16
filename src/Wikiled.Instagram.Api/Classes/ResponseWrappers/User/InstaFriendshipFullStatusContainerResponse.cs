using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFriendshipFullStatusContainerResponse
    {
        [JsonProperty("friendship_status")]
        public InstaFriendshipFullStatusResponse FriendshipStatus { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}