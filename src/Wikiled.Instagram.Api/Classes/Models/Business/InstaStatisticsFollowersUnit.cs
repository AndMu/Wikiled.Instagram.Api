using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatisticsFollowersUnit
    {
        public List<InstaStatisticsDataPointItem> AllFollowersAgeGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> DaysHourlyFollowersGraphs { get; set; } = new List<InstaStatisticsDataPointItem>();

        public int FollowersDeltaFromLastWeek { get; set; }

        public List<InstaStatisticsDataPointItem> FollowersTopCitiesGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> FollowersTopCountriesGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public string FollowersUnitState { get; set; }

        public List<InstaStatisticsDataPointItem> GenderGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> MenFollowersAgeGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> TodayHourlyGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> WeekDailyFollowersGraph { get; set; } = new List<InstaStatisticsDataPointItem>();

        public List<InstaStatisticsDataPointItem> WomenFollowersAgeGraph { get; set; } = new List<InstaStatisticsDataPointItem>();
    }
}
