namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatistics
    {
        public InstaStatisticsBusinessManager BusinessManager { get; set; }

        public string BusinessProfileId { get; set; }

        public int FollowersCount { get; set; } = 0;

        public string Id { get; set; }

        public string ProfilePicture { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }
    }
}