using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsTopPostsUnitResponse
    {
        [JsonProperty("last_week_posts_count")]
        public long? LastWeekPostsCount { get; set; } = 0;

        [JsonProperty("summary_posts")]
        public InstaStatisticsTopPostsResponse SummaryPosts { get; set; }

        [JsonProperty("top_posts")]
        public InstaStatisticsTopPostsResponse TopPosts { get; set; }

        [JsonProperty("week_over_week_posts_delta")]
        public long? WeekOverWeekPostsDelta { get; set; } = 0;
    }
}