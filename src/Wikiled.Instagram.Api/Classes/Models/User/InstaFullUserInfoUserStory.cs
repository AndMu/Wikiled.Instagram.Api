using Wikiled.Instagram.Api.Classes.Models.Broadcast;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFullUserInfoUserStory
    {
        public InstaBroadcastList Broadcast { get; set; }

        public InstaFullUserInfoUserStoryReel Reel { get; set; }
    }
}