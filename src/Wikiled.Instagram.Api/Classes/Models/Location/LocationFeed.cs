using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;

namespace Wikiled.Instagram.Api.Classes.Models.Location
{
    public class LocationFeed : InstaBaseFeed
    {
        public Location Location { get; set; }

        public long MediaCount { get; set; }

        public InstaMediaList RankedMedias { get; set; } = new InstaMediaList();

        public InstaStory Story { get; set; }
    }
}