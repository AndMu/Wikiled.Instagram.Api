﻿using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaBlockedUserInfoConverter : IObjectConverter<InstaBlockedUserInfo, InstaBlockedUserInfoResponse>
    {
        public InstaBlockedUserInfoResponse SourceObject { get; set; }

        public InstaBlockedUserInfo Convert()
        {
            return new InstaBlockedUserInfo
            {
                BlockedAt = SourceObject.BlockedAt,
                FullName = SourceObject.FullName,
                IsPrivate = SourceObject.IsPrivate,
                Pk = SourceObject.Pk,
                ProfilePicture = SourceObject.ProfilePicture,
                UserName = SourceObject.UserName
            };
        }
    }
}