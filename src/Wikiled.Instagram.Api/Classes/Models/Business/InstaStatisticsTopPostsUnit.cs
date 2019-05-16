using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatisticsTopPostsUnit
    {
        public long LastWeekPostsCount { get; set; } = 0;

        public List<InstaMediaShort> SummaryPosts { get; set; } = new List<InstaMediaShort>();

        public List<InstaMediaShort> TopPosts { get; set; } = new List<InstaMediaShort>();

        public long WeekOverWeekPostsDelta { get; set; } = 0;
    }
}