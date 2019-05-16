using System;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaAccountDetails
    {
        public InstaAdsInfo AdsInfo { get; set; }

        public DateTime DateJoined { get; set; }

        public bool HasFormerUsernames { get; set; } = false;

        public bool HasSharedFollowerAccounts { get; set; } = false;

        public InstaPrimaryCountryInfo PrimaryCountryInfo { get; set; }
    }
}
