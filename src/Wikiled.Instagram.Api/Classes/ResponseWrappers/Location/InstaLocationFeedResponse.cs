using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class InstaLocationFeedResponse : BaseLoadableResponse
    {
        [JsonProperty("items")] public List<InstaMediaItemResponse> Items { get; set; } = new List<InstaMediaItemResponse>();

        [JsonProperty("location")] public InstaLocationResponse Location { get; set; }

        [JsonProperty("media_count")] public long MediaCount { get; set; }

        [JsonProperty("ranked_items")] public List<InstaMediaItemResponse> RankedItems { get; set; } = new List<InstaMediaItemResponse>();

        [JsonProperty("story")] public InstaStoryResponse Story { get; set; }
    }
}
