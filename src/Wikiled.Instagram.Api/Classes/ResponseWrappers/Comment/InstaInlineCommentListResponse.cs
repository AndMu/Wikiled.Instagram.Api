using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaInlineCommentListResponse : InstaBaseStatusResponse
    {
        [JsonProperty("child_comment_count")]
        public int ChildCommentCount { get; set; }

        [JsonProperty("child_comments")]
        public List<InstaCommentResponse> ChildComments { get; set; }

        [JsonProperty("has_more_head_child_comments")]
        public bool HasMoreHeadChildComments { get; set; }

        [JsonProperty("has_more_tail_child_comments")]
        public bool HasMoreTailChildComments { get; set; }

        [JsonProperty("next_max_child_cursor")]
        public string NextMaxId { get; set; }

        [JsonProperty("next_in_child_cursor")]
        public string NextMinId { get; set; }

        [JsonProperty("num_tail_child_comments")]
        public int NumTailChildComments { get; set; }

        [JsonProperty("parent_comment")]
        public InstaCommentResponse ParentComment { get; set; }
    }
}