using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.TV
{
    public class InstaTVSearchResultResponse
    {
        [JsonProperty("channel")] public InstaTVChannelResponse Channel { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("user")] public InstaUserShortFriendshipResponse User { get; set; }
    }
}
