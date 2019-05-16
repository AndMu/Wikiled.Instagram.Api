namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaFullMediaInsightsMetrics
    {
        public int ImpressionCount { get; set; }

        public InstaFullMediaInsightsNodeItem Impressions { get; set; }

        public int OwnerAccountFollowsCount { get; set; }

        public int OwnerProfileViewsCount { get; set; }

        public InstaFullMediaInsightsNodeItem ProfileActions { get; set; }

        public InstaFullMediaInsightsNodeItem Reach { get; set; }

        public int ReachCount { get; set; }

        public string State { get; set; }
    }
}
