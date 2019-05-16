using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaVoiceUserResponse
    {
        [JsonProperty("friendship_status")]
        public InstaFriendshipStatusResponse FriendshipStatus { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }
    }
}