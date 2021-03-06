﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class
        InstaAnimatedImageUserConverter : IObjectConverter<InstaAnimatedImageUser, InstaAnimatedImageUserResponse>
    {
        public InstaAnimatedImageUserResponse SourceObject { get; set; }

        public InstaAnimatedImageUser Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var user = new InstaAnimatedImageUser
            {
                IsVerified = SourceObject.IsVerified, Username = SourceObject.Username
            };

            return user;
        }
    }
}