using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFullUserInfoUserStoryResponse
    {
        [JsonProperty("broadcast")] public InstaBroadcastSuggestedResponse Broadcast { get; set; }

        [JsonProperty("reel")] public InstaFullUserInfoUserStoryReelResponse Reel { get; set; }
    }
}
