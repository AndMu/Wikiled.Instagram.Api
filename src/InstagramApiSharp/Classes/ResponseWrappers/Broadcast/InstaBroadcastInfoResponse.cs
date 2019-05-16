﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastInfoResponse
    {
        [JsonProperty("broadcast_message")] public string BroadcastMessage { get; set; }

        [JsonProperty("broadcast_owner")] public InstaUserShortFriendshipFullResponse BroadcastOwner { get; set; }

        [JsonProperty("broadcast_status")] public string BroadcastStatus { get; set; }

        [JsonProperty("cover_frame_url")] public string CoverFrameUrl { get; set; }

        [JsonProperty("dash_manifest")] public string DashManifest { get; set; }

        [JsonProperty("encoding_tag")] public string EncodingTag { get; set; }

        [JsonProperty("expire_at")] public long? ExpireAt { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("internal_only")] public bool InternalOnly { get; set; }

        [JsonProperty("media_id")] public string MediaId { get; set; }

        [JsonProperty("number_of_qualities")] public int NumberOfQualities { get; set; }

        [JsonProperty("organic_tracking_token")]
        public string OrganicTrackingToken { get; set; }

        [JsonProperty("published_time")] public long? PublishedTime { get; set; }

        [JsonProperty("total_unique_viewer_count")]
        public int TotalUniqueViewerCount { get; set; }
    }
}
