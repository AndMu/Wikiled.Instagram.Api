using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatisticsAccountInsightsUnit
    {
        public List<InstaStatisticsDataPointItem> AccountActionsLastWeekDailyGraph { get; set; } =
            new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> AccountDiscoveryLastWeekDailyGraph { get; set; } =
            new List<InstaStatisticsDataPointItem>();

        public InstaStatisticsInsightsChannel InstagramAccountInsightsChannel { get; set; }

        public int LastWeekCall { get; set; } = 0;

        public int LastWeekEmail { get; set; } = 0;

        public int LastWeekGetDirection { get; set; } = 0;

        public int LastWeekImpressions { get; set; } = 0;

        public int LastWeekProfileVisits { get; set; } = 0;

        public int LastWeekReach { get; set; } = 0;

        public int LastWeekText { get; set; } = 0;

        public int LastWeekWebsiteVisits { get; set; } = 0;

        public int WeekOverWeekCall { get; set; } = 0;

        public int WeekOverWeekEmail { get; set; } = 0;

        public int WeekOverWeekGetDirection { get; set; } = 0;

        public int WeekOverWeekImpressions { get; set; } = 0;

        public int WeekOverWeekProfileVisits { get; set; } = 0;

        public int WeekOverWeekReach { get; set; } = 0;

        public int WeekOverWeekText { get; set; } = 0;

        public int WeekOverWeekWebsiteVisits { get; set; } = 0;
    }
}