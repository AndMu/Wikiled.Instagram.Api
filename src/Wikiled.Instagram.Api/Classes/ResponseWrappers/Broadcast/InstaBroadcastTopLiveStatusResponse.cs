using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastTopLiveStatusResponse
    {
        [JsonProperty("broadcast_status_items")]
        public List<InstaBroadcastStatusItemResponse> BroadcastStatusItems { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}