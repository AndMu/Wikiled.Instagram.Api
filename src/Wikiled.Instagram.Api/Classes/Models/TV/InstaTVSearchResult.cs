using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTvSearchResult
    {
        public InstaTvChannel Channel { get; set; }

        public string Type { get; set; }

        public InstaUserShortFriendship User { get; set; }
    }
}