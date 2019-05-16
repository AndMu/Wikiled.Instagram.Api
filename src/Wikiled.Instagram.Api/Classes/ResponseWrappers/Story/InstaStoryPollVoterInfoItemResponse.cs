using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryPollVoterInfoItemResponse
    {
        [JsonProperty("latest_poll_vote_time")]
        public long? LatestPollVoteTime { get; set; }

        [JsonProperty("max_id")]
        public string MaxId { get; set; }

        [JsonProperty("more_available")]
        public bool MoreAvailable { get; set; }

        [JsonProperty("poll_id")]
        public long PollId { get; set; }

        [JsonProperty("voters")]
        public List<InstaStoryVoterItemResponse> Voters { get; set; }
    }
}