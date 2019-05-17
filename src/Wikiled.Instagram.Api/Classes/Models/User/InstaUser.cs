namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUser : UserShortDescription
    {
        public InstaUser(UserShortDescription instaUserShortDescription)
        {
            Pk = instaUserShortDescription.Pk;
            UserName = instaUserShortDescription.UserName;
            FullName = instaUserShortDescription.FullName;
            IsPrivate = instaUserShortDescription.IsPrivate;
            ProfilePicture = instaUserShortDescription.ProfilePicture;
            ProfilePictureId = instaUserShortDescription.ProfilePictureId;
            IsVerified = instaUserShortDescription.IsVerified;
        }

        public int FollowersCount { get; set; }

        public string FollowersCountByLine { get; set; }

        public InstaFriendshipShortStatus FriendshipStatus { get; set; }

        public bool HasAnonymousProfilePicture { get; set; }

        public int MutualFollowers { get; set; }

        public string SearchSocialContext { get; set; }

        public string SocialContext { get; set; }

        public int UnseenCount { get; set; }
    }
}