using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastLiveHeartBeatViewerCountResponse
    {
        [JsonProperty("broadcast_status")]
        public string BroadcastStatus { get; set; }

        [JsonProperty("cobroadcaster_ids")]
        public object[] CobroadcasterIds { get; set; }

        [JsonProperty("is_top_live_eligible")]
        public int IsTopLiveEligible { get; set; }

        [JsonProperty("offset_to_video_start")]
        public int OffsetToVideoStart { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total_unique_viewer_count")]
        public int TotalUniqueViewerCount { get; set; }

        [JsonProperty("viewer_count")]
        public float ViewerCount { get; set; }
    }
}