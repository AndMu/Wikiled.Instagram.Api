using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightShortListResponse : InstaDefault
    {
        public List<InstaHighlightShortResponse> Items { get; set; }

        [JsonProperty("max_id")] public string MaxId { get; set; }

        [JsonProperty("more_available")] public bool MoreAvailable { get; set; }

        [JsonProperty("num_results")] public int ResultsCount { get; set; }
    }
}
