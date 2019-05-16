﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed
{
    public class InstaFeedResponse : BaseLoadableResponse
    {
        [JsonProperty("is_direct_v2_enabled")] public bool IsDirectV2Enabled { get; set; }

        [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
        public List<InstaMediaItemResponse> Items { get; set; } = new List<InstaMediaItemResponse>();

        //[JsonProperty("suggested_users")]
        [JsonIgnore] public List<InstaSuggestionItemResponse> SuggestedUsers { get; set; } = new List<InstaSuggestionItemResponse>();
    }
}
