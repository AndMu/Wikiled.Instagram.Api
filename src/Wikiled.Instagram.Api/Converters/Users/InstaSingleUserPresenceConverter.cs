using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaSingleUserPresenceConverter : IObjectConverter<InstaUserPresence, InstaUserPresenceResponse>
    {
        public InstaUserPresenceResponse SourceObject { get; set; }

        public InstaUserPresence Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var userPresence = new InstaUserPresence
            {
                Pk = SourceObject.Pk,
                IsActive = SourceObject.IsActive ?? false,
                LastActivity = (SourceObject.LastActivityAtMs ?? 0).FromUnixTimeMiliSeconds()
            };
            return userPresence;
        }
    }
}