using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastSuggestedResponse
    {
        [JsonProperty("broadcasts")] public List<InstaBroadcastResponse> Broadcasts { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
