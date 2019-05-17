using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaUserShortFriendshipConverter : IObjectConverter<InstaUserShortDescriptionFriendship, InstaUserShortFriendshipResponse>
    {
        public InstaUserShortFriendshipResponse SourceObject { get; set; }

        public InstaUserShortDescriptionFriendship Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var user = new InstaUserShortDescriptionFriendship
            {
                Pk = SourceObject.Pk,
                UserName = SourceObject.UserName,
                FullName = SourceObject.FullName,
                IsPrivate = SourceObject.IsPrivate,
                ProfilePicture = SourceObject.ProfilePicture,
                ProfilePictureId = SourceObject.ProfilePictureId,
                IsVerified = SourceObject.IsVerified,
                ProfilePicUrl = SourceObject.ProfilePicture
            };
            if (SourceObject.FriendshipStatus != null)
            {
                var item = SourceObject.FriendshipStatus;
                var friend = new InstaFriendshipShortStatus
                {
                    Following = item.Following,
                    IncomingRequest = item.IncomingRequest,
                    IsBestie = item.IsBestie,
                    IsPrivate = item.IsPrivate,
                    OutgoingRequest = item.OutgoingRequest,
                    Pk = 0
                };
                user.FriendshipStatus = friend;
            }

            return user;
        }
    }
}