using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    internal class InstaBroadcastStartResponse
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}