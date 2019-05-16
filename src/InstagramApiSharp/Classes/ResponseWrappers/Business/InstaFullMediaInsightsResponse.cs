﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsResponse
    {
        [JsonProperty("comment_count")] public int? CommentCount { get; set; }

        [JsonProperty("creation_time")] public long? CreationTime { get; set; }

        [JsonProperty("display_url")] public string DisplayUrl { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("inline_insights_node")] public InstaFullMediaInsightsInlineNodeResponse InlineInsightsNode { get; set; }

        [JsonProperty("instagram_media_type")] public string InstagramMediaType { get; set; }

        [JsonProperty("like_count")] public int? LikeCount { get; set; }

        [JsonProperty("save_count")] public int? SaveCount { get; set; }
    }
}
