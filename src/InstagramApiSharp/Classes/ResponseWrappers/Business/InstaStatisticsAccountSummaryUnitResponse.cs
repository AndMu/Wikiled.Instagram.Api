using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsAccountSummaryUnitResponse
    {
        [JsonProperty("followers_count")] public long? FollowersCount { get; set; } = 0;

        [JsonProperty("followers_delta_from_last_week")]
        public long? FollowersDeltaFromLastWeek { get; set; } = 0;

        [JsonProperty("posts_count")] public long? PostsCount { get; set; } = 0;

        [JsonProperty("posts_delta_from_last_week")]
        public long? PostsDeltaFromLastWeek { get; set; } = 0;
    }
}
