﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaDirectHashtagResponse
    {
        [JsonProperty("media")] public InstaMediaItemResponse Media { get; set; }

        [JsonProperty("media_count")] public long MediaCount { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }
}
