﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;

namespace Wikiled.Instagram.Api.Converters.Other
{
    internal class InstaPresenceConverter : IObjectConverter<InstaPresence, InstaPresenceResponse>
    {
        public InstaPresenceResponse SourceObject { get; set; }

        public InstaPresence Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var presence = new InstaPresence
            {
                PresenceDisabled = SourceObject.Disabled ?? false,
                ThreadPresenceDisabled = SourceObject.ThreadPresenceDisabled ?? false
            };

            return presence;
        }
    }
}