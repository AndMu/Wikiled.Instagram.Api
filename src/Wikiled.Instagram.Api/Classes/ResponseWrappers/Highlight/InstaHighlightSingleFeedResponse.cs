using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightSingleFeedResponse : InstaHighlightFeedResponse
    {
        [JsonProperty("items")]
        public List<InstaStoryItemResponse> Items { get; set; } = new List<InstaStoryItemResponse>();
    }
}