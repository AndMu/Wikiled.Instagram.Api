using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsStoriesUnitResponse
    {
        [JsonProperty("last_week_stories_count")]
        public long? LastWeekStoriesCount { get; set; } = 0;

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("summary_stories")]
        public InstaStatisticsSummaryStoriesResponse SummaryStories { get; set; }

        [JsonProperty("week_over_week_stories_delta")]
        public long? WeekOverWeekStoriesDelta { get; set; } = 0;
    }
}