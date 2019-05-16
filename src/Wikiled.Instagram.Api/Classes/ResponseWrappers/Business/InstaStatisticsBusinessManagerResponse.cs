﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsBusinessManagerResponse
    {
        [JsonProperty("account_insights_unit")]
        public InstaStatisticsAccountInsightsUnitResponse AccountInsightsUnit { get; set; }

        [JsonProperty("account_summary_unit")]
        public InstaStatisticsAccountSummaryUnitResponse AccountSummaryUnit { get; set; }

        [JsonProperty("followers_unit")]
        public InstaStatisticsFollowersUnitResponse FollowersUnit { get; set; }

        [JsonProperty("promotions_unit")]
        public InstaStatisticsPromotionsUnitResponse PromotionsUnit { get; set; }

        [JsonProperty("stories_unit")]
        public InstaStatisticsStoriesUnitResponse StoriesUnit { get; set; }

        [JsonProperty("top_posts_unit")]
        public InstaStatisticsTopPostsUnitResponse TopPostsUnit { get; set; }
    }
}