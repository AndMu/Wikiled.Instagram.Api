using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaUserConverter : IObjectConverter<InstaUser, InstaUserResponse>
    {
        public InstaUserResponse SourceObject { get; set; }

        public InstaUser Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var shortConverter = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject);
            var user = new InstaUser(shortConverter.Convert())
            {
                HasAnonymousProfilePicture = SourceObject.HasAnonymousProfilePicture,
                FollowersCount = SourceObject.FollowersCount,
                FollowersCountByLine = SourceObject.FollowersCountByLine,
                SearchSocialContext = SourceObject.SearchSocialContext,
                SocialContext = SourceObject.SocialContext
            };

            if (double.TryParse(SourceObject.MulualFollowersCount, out var mutualFollowers))
            {
                user.MutualFollowers = System.Convert.ToInt16(mutualFollowers);
            }

            if (SourceObject.FriendshipStatus != null)
            {
                var freindShipStatusConverter =
                    InstaConvertersFabric.Instance.GetSingleFriendshipShortStatusConverter(SourceObject.FriendshipStatus);
                user.FriendshipStatus = freindShipStatusConverter.Convert();
            }

            return user;
        }
    }
}