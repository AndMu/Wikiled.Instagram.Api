﻿using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastResponse
    {
        [JsonProperty("broadcast_message")]
        public string BroadcastMessage { get; set; }

        [JsonProperty("broadcast_owner")]
        public InstaUserShortFriendshipFullResponse BroadcastOwner { get; set; }

        [JsonProperty("broadcast_status")]
        public string BroadcastStatus { get; set; }

        [JsonProperty("cover_frame_url")]
        public string CoverFrameUrl { get; set; }

        [JsonProperty("dash_abr_playback_url")]
        public string DashAbrPlaybackUrl { get; set; }

        [JsonProperty("dash_manifest")]
        public string DashManifest { get; set; }

        [JsonProperty("dash_playback_url")]
        public string DashPlaybackUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("internal_only")]
        public bool InternalOnly { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("organic_tracking_token")]
        public string OrganicTrackingToken { get; set; }

        [JsonProperty("published_time")]
        public long? PublishedTime { get; set; }

        [JsonProperty("rtmp_playback_url")]
        public string RtmpPlaybackUrl { get; set; }

        [JsonProperty("viewer_count")]
        public long ViewerCount { get; set; }
    }
}