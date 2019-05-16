using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaFriendshipShortStatusConverter : IObjectConverter<InstaFriendshipShortStatus,
            InstaFriendshipShortStatusResponse>
    {
        public InstaFriendshipShortStatusResponse SourceObject { get; set; }

        public InstaFriendshipShortStatus Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var friendships = new InstaFriendshipShortStatus
            {
                Following = SourceObject.Following,
                IncomingRequest = SourceObject.IncomingRequest,
                IsBestie = SourceObject.IsBestie,
                IsPrivate = SourceObject.IsPrivate,
                OutgoingRequest = SourceObject.OutgoingRequest
            };

            return friendships;
        }
    }
}