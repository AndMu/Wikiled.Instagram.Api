using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryCountdownListResponse
    {
        [JsonProperty("countdowns")]
        public List<InstaStoryCountdownStickerItemResponse> Items { get; set; }

        [JsonProperty("max_id")]
        public string MaxId { get; set; }

        [JsonProperty("more_available")]
        public bool? MoreAvailable { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}