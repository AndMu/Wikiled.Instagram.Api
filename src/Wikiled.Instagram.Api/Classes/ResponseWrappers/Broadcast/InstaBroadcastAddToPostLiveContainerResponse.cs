using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastAddToPostLiveContainerResponse
    {
        [JsonProperty("post_live_items")] public List<InstaBroadcastAddToPostLiveResponse> PostLiveItems { get; set; }
    }
}
