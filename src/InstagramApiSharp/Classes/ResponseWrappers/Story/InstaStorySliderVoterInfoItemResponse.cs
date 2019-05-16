using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStorySliderVoterInfoItemResponse
    {
        [JsonProperty("latest_slider_vote_time")]
        public long? LatestSliderVoteTime { get; set; }

        [JsonProperty("max_id")] public string MaxId { get; set; }

        [JsonProperty("more_available")] public bool MoreAvailable { get; set; }

        [JsonProperty("slider_id")] public long SliderId { get; set; }

        [JsonProperty("voters")] public List<InstaStoryVoterItemResponse> Voters { get; set; }
    }
}
