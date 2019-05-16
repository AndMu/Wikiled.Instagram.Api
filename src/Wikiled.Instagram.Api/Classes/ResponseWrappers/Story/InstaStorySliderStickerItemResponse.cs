using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStorySliderStickerItemResponse
    {
        [JsonProperty("emoji")] public string Emoji { get; set; }

        [JsonProperty("question")] public string Question { get; set; }

        [JsonProperty("slider_id")] public long SliderId { get; set; }

        [JsonProperty("slider_vote_average")] public double? SliderVoteAverage { get; set; }

        [JsonProperty("slider_vote_count")] public long? SliderVoteCount { get; set; }

        [JsonProperty("text_color")] public string TextColor { get; set; }

        [JsonProperty("viewer_can_vote")] public bool ViewerCanVote { get; set; }
    }
}
