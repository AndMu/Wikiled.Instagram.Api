namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFullUserInfo
    {
        public InstaFullUserInfoUserFeed Feed { get; set; }

        public InstaFullUserInfoUserStoryReel ReelFeed { get; set; }

        public InstaUserInfo UserDetail { get; set; }

        public InstaFullUserInfoUserStory UserStory { get; set; }

        internal string Status { get; set; }
    }
}