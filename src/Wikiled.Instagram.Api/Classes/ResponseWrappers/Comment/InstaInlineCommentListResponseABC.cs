﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    internal class InstaInlineCommentListResponseAbc
    {
        [JsonProperty("can_view_more_preview_comments")]
        public bool CanViewMorePreviewComments { get; set; }

        [JsonProperty("caption")]
        public InstaCaptionResponse Caption { get; set; }

        [JsonProperty("caption_is_edited")]
        public bool CaptionIsEdited { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("comment_likes_enabled")]
        public bool CommentLikesEnabled { get; set; }

        [JsonProperty("comments")]
        public List<InstaCommentResponse> Comments { get; set; }

        [JsonProperty("has_more_comments")]
        public bool HasMoreComments { get; set; }

        [JsonProperty("has_more_headload_comments")]
        public bool HasMoreHeadloadComments { get; set; }

        [JsonProperty("initiate_at_top")]
        public bool InitiateAtTop { get; set; }

        [JsonProperty("insert_new_comment_to_top")]
        public bool InsertNewCommentToTop { get; set; }

        [JsonProperty("media_header_display")]
        public string MediaHeaderDisplay { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }

        [JsonProperty("next_min_id")]
        public string NextMinId { get; set; }

        //[JsonProperty("quick_response_emojis")]
        //public object[] quick_response_emojis { get; set; }
        [JsonProperty("preview_comments")]
        public List<InstaCommentResponse> PreviewComments { get; set; }

        [JsonProperty("threading_enabled")]
        public bool ThreadingEnabled { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}