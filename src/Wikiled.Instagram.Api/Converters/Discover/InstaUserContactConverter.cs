﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaUserContactConverter : IObjectConverter<InstaUserContact, InstaUserContactResponse>
    {
        public InstaUserContactResponse SourceObject { get; set; }

        public InstaUserContact Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var user = new InstaUserContact
            {
                Pk = SourceObject.Pk,
                UserName = SourceObject.UserName,
                FullName = SourceObject.FullName,
                IsPrivate = SourceObject.IsPrivate,
                ProfilePicture = SourceObject.ProfilePicture,
                ProfilePictureId = SourceObject.ProfilePictureId,
                IsVerified = SourceObject.IsVerified,
                ExtraDisplayName = SourceObject.ExtraDisplayName,
                ProfilePicUrl = SourceObject.ProfilePicture
            };
            return user;
        }
    }
}