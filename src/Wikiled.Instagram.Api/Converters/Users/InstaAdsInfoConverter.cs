﻿using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaAdsInfoConverter : IObjectConverter<InstaAdsInfo, InstaAdsInfoResponse>
    {
        public InstaAdsInfoResponse SourceObject { get; set; }

        public InstaAdsInfo Convert()
        {
            return new InstaAdsInfo { AdsUrl = SourceObject.AdsUrl, HasAds = SourceObject.HasAds ?? false };
        }
    }
}