using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastStatusItemResponse
    {
        [JsonProperty("broadcast_status")]
        public string BroadcastStatus { get; set; }

        [JsonProperty("cover_frame_url")]
        public string CoverFrameUrl { get; set; }

        [JsonProperty("has_reduced_visibility")]
        public bool HasReducedVisibility { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("viewer_count")]
        public float ViewerCount { get; set; }
    }
}