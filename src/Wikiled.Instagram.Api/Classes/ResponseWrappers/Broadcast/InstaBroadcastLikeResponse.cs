using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastLikeResponse
    {
        [JsonProperty("burst_likes")]
        public int BurstLikes { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("like_ts")]
        public long? LikeTs { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}