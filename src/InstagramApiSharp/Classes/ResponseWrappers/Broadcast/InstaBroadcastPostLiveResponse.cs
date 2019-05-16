using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastPostLiveResponse
    {
        [JsonProperty("broadcasts")] public List<InstaBroadcastInfoResponse> Broadcasts { get; set; }

        [JsonProperty("peak_viewer_count")] public int PeakViewerCount { get; set; }

        [JsonProperty("pk")] public string Pk { get; set; }

        [JsonProperty("user")] public InstaUserShortFriendshipFullResponse User { get; set; }
    }
}
