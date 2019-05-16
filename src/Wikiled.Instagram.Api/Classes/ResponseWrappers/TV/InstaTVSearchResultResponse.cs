using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTvSearchResultResponse
    {
        [JsonProperty("channel")]
        public InstaTvChannelResponse Channel { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public InstaUserShortFriendshipResponse User { get; set; }
    }
}