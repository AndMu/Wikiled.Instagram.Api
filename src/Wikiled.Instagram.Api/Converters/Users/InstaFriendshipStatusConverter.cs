﻿using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaFriendshipStatusConverter : IObjectConverter<InstaFriendshipStatus, InstaFriendshipStatusResponse>
    {
        public InstaFriendshipStatusResponse SourceObject { get; set; }

        public InstaFriendshipStatus Convert()
        {
            var friendShip = new InstaFriendshipStatus
            {
                Following = SourceObject.Following,
                Blocking = SourceObject.Blocking,
                FollowedBy = SourceObject.FollowedBy,
                OutgoingRequest = SourceObject.OutgoingRequest,
                IsBlockingReel = SourceObject.IsBlockingReel ?? false
            };
            friendShip.IncomingRequest = SourceObject.IncomingRequest;
            friendShip.IsPrivate = SourceObject.IsPrivate;
            return friendShip;
        }
    }
}