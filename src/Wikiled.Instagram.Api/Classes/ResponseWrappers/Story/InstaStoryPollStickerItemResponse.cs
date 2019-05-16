using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryPollStickerItemResponse
    {
        [JsonProperty("finished")]
        public bool Finished { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_shared_result")]
        public bool IsSharedResult { get; set; }

        [JsonProperty("poll_id")]
        public long PollId { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("tallies")]
        public List<InstaStoryTalliesItemResponse> Tallies { get; set; }

        [JsonProperty("viewer_can_vote")]
        public bool ViewerCanVote { get; set; }

        [JsonProperty("viewer_vote")]
        public long? ViewerVote { get; set; }
    }
}