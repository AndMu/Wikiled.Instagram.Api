﻿using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaUserLookupConverter : IObjectConverter<InstaUserLookup, InstaUserLookupResponse>
    {
        public InstaUserLookupResponse SourceObject { get; set; }

        public InstaUserLookup Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var lookup = new InstaUserLookup
            {
                CanEmailReset = SourceObject.CanEmailReset,
                CanSmsReset = SourceObject.CanSmsReset,
                CanWaReset = SourceObject.CanWaReset,
                CorrectedInput = SourceObject.CorrectedInput,
                Email = SourceObject.Email,
                EmailSent = SourceObject.EmailSent,
                HasValidPhone = SourceObject.HasValidPhone,
                MultipleUsersFound = SourceObject.MultipleUsersFound,
                PhoneNumber = SourceObject.PhoneNumber,
                SmsSent = SourceObject.SmsSent
            };
            try
            {
                if (!string.IsNullOrEmpty(SourceObject.LookupSource))
                {
                    lookup.LookupSourceType =
                        (InstaLookupType)Enum.Parse(typeof(InstaLookupType), SourceObject.LookupSource, true);
                }
            }
            catch
            {
            }

            try
            {
                if (SourceObject.User != null)
                {
                    lookup.User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert();
                }
            }
            catch
            {
            }

            return lookup;
        }
    }
}