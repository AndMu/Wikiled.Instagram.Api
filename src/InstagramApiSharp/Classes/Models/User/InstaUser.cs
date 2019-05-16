﻿namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUser : InstaUserShort
    {
        public InstaUser(InstaUserShort instaUserShort)
        {
            Pk = instaUserShort.Pk;
            UserName = instaUserShort.UserName;
            FullName = instaUserShort.FullName;
            IsPrivate = instaUserShort.IsPrivate;
            ProfilePicture = instaUserShort.ProfilePicture;
            ProfilePictureId = instaUserShort.ProfilePictureId;
            IsVerified = instaUserShort.IsVerified;
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
