using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastAddToPostLiveResponse
    {
        [JsonProperty("broadcasts")] public List<InstaBroadcastResponse> Broadcasts { get; set; } = new List<InstaBroadcastResponse>();

        [JsonProperty("can_reply")] public bool CanReply { get; set; }

        [JsonProperty("dash_manifest")] public string DashManifest { get; set; }

        [JsonProperty("last_seen_broadcast_ts")]
        public double? LastSeenBroadcastTs { get; set; }

        [JsonProperty("pk")] public string Pk { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("user")] public InstaUserShortFriendshipFullResponse User { get; set; }
    }
}
