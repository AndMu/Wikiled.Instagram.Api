using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;

namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaExploreFeed : InstaBaseFeed
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public InstaChannel Channel { get; set; } = new InstaChannel();

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }

        public string RankToken { get; set; }

        public int ResultsCount { get; set; }

        public InstaStoryTray StoryTray { get; set; } = new InstaStoryTray();
    }
}