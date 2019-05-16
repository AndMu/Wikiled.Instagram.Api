using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaCommentShortResponse
    {
        [JsonProperty("comment_like_count")]
        public int CommentLikeCount { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("created_at")]
        public float CreatedAt { get; set; }

        [JsonProperty("created_at_utc")]
        public int CreatedAtUtc { get; set; }

        [JsonProperty("has_liked_comment")]
        public bool HasLikedComment { get; set; }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("parent_comment_id")]
        public long ParentCommentId { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }
    }
}