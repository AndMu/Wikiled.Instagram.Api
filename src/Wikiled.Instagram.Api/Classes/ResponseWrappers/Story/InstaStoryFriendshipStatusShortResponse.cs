using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryFriendshipStatusShortResponse
    {
        [JsonProperty("following")] public bool Following { get; set; }

        [JsonProperty("is_muting_reel")] public bool? IsMutingReel { get; set; }

        [JsonProperty("muting")] public bool? Muting { get; set; }

        [JsonProperty("outgoing_request")] public bool? OutgoingRequest { get; set; }
    }
}
