using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightFeedsResponse
    {
        [JsonProperty("tray")] public List<InstaHighlightFeedResponse> Items { get; set; } = new List<InstaHighlightFeedResponse>();

        [JsonProperty("show_empty_state")] public bool? ShowEmptyState { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
