﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaCommentResponse
    {
        [JsonProperty("bit_flags")]
        public int BitFlags { get; set; }

        [JsonProperty("child_comment_count")]
        public int ChildCommentCount { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("created_at_utc")]
        public string CreatedAtUtc { get; set; }

        [JsonProperty("did_report_as_spam")]
        public bool DidReportAsSpam { get; set; }

        [JsonProperty("has_liked_comment")]
        public bool HasLikedComment { get; set; }

        [JsonProperty("has_more_head_child_comments")]
        public bool HasMoreHeadChildComments { get; set; }

        //[JsonProperty("num_tail_child_comments")] public int NumTailChildComments { get; set; }

        [JsonProperty("has_more_tail_child_comments")]
        public bool HasMoreTailChildComments { get; set; }

        [JsonProperty("comment_like_count")]
        public int LikesCount { get; set; }

        [JsonProperty("other_preview_users")]
        public List<InstaUserShortResponse> OtherPreviewUsers { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }

        //[JsonProperty("next_max_child_cursor")] public string NextMaxChildCursor { get; set; }

        [JsonProperty("preview_child_comments")]
        public List<InstaCommentShortResponse> PreviewChildComments { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}