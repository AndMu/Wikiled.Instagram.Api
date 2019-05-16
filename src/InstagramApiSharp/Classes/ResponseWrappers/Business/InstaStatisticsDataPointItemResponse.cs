﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataPointItemResponse
    {
        [JsonProperty("label")] public string Label { get; set; }

        [JsonProperty("value")] public int? Value { get; set; } = 0;
    }
}
