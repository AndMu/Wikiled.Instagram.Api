using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryFriendshipStatusResponse
    {
        [JsonProperty("blocking")] public bool? Blocking { get; set; }

        [JsonProperty("followed_by")] public bool FollowedBy { get; set; }

        [JsonProperty("following")] public bool Following { get; set; }

        [JsonProperty("incoming_request")] public bool? IncomingRequest { get; set; }

        [JsonProperty("is_bestie")] public bool? IsBestie { get; set; }

        [JsonProperty("is_blocking_reel")] public bool? IsBlockingReel { get; set; }

        [JsonProperty("is_muting_reel")] public bool? IsMutingReel { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("muting")] public bool? Muting { get; set; }

        [JsonProperty("outgoing_request")] public bool? OutgoingRequest { get; set; }
    }
}
