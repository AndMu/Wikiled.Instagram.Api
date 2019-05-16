using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsMetricsResponse
    {
        [JsonProperty("impression_count")] public int? ImpressionCount { get; set; }

        [JsonProperty("impressions")] public InstaFullMediaInsightsImpressionsResponse Impressions { get; set; }

        [JsonProperty("owner_account_follows_count")]
        public int? OwnerAccountFollowsCount { get; set; }

        [JsonProperty("owner_profile_views_count")]
        public int? OwnerProfileViewsCount { get; set; }

        [JsonProperty("profile_actions")] public InstaFullMediaInsightsProfileActionsResponse ProfileActions { get; set; }

        [JsonProperty("reach")] public InstaFullMediaInsightsReachResponse Reach { get; set; }

        [JsonProperty("reach_count")] public int? ReachCount { get; set; }
    }
}
