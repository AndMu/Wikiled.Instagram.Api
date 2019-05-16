namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBlockedUserInfo
    {
        public long BlockedAt { get; set; }

        public string FullName { get; set; }

        public bool IsPrivate { get; set; }

        public long Pk { get; set; }

        public string ProfilePicture { get; set; }

        public string UserName { get; set; }
    }
}