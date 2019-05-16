namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatisticsStoriesUnit
    {
        public long LastWeekStoriesCount { get; set; } = 0;

        public string State { get; set; }

        public InstaStatisticsSummaryStories SummaryStories { get; set; }

        public long WeekOverWeekStoriesDelta { get; set; } = 0;
    }
}