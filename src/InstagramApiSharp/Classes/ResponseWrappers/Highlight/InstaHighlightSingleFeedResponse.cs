using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightSingleFeedResponse : InstaHighlightFeedResponse
    {
        [JsonProperty("items")] public List<InstaStoryItemResponse> Items { get; set; } = new List<InstaStoryItemResponse>();
    }
}
