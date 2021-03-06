﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaRecentActivityFeedResponse
    {
        [JsonProperty("args")]
        public InstaRecentActivityStoryItemResponse Args { get; set; }

        [JsonProperty("pk")]
        public string Pk { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}