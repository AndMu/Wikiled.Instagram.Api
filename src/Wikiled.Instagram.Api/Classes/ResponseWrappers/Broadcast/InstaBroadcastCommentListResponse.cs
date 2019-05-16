using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastCommentListResponse
    {
        [JsonProperty("caption")] public InstaCaptionResponse Caption { get; set; }

        [JsonProperty("caption_is_edited")] public bool? CaptionIsEdited { get; set; }

        [JsonProperty("comment_count")] public int? CommentCount { get; set; }

        [JsonProperty("comment_likes_enabled")]
        public bool? CommentLikesEnabled { get; set; }

        [JsonProperty("comment_muted")] public int? CommentMuted { get; set; }

        [JsonProperty("comments")] public List<InstaBroadcastCommentResponse> Comments { get; set; }

        [JsonProperty("has_more_comments")] public bool? HasMoreComments { get; set; }

        [JsonProperty("has_more_headload_comments")]
        public bool? HasMoreHeadloadComments { get; set; }

        [JsonProperty("is_first_fetch")] public string IsFirstFetch { get; set; }

        [JsonProperty("live_seconds_per_comment")]
        public int? LiveSecondsPerComment { get; set; }

        [JsonProperty("media_header_display")] public string MediaHeaderDisplay { get; set; }

        [JsonProperty("pinned_comment")] public InstaBroadcastCommentResponse PinnedComment { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("system_comments")] public object SystemComments { get; set; }
    }
}
