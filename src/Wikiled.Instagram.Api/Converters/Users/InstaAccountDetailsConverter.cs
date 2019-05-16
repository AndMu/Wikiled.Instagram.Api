using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaAccountDetailsConverter : IObjectConverter<InstaAccountDetails, InstaAccountDetailsResponse>
    {
        public InstaAccountDetailsResponse SourceObject { get; set; }

        public InstaAccountDetails Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var details = new InstaAccountDetails
            {
                DateJoined = InstaDateTimeHelper.FromUnixTimeSeconds(SourceObject.DateJoined ?? 0)
            };
            if (SourceObject.FormerUsernameInfo != null)
            {
                details.HasFormerUsernames = SourceObject.FormerUsernameInfo.HasFormerUsernames ?? false;
            }

            if (SourceObject.SharedFollowerAccountsInfo != null)
            {
                details.HasSharedFollowerAccounts =
                    SourceObject.SharedFollowerAccountsInfo.HasSharedFollowerAccounts ?? false;
            }

            if (SourceObject.AdsInfo != null)
            {
                try
                {
                    details.AdsInfo = InstaConvertersFabric.Instance.GetAdsInfoConverter(SourceObject.AdsInfo).Convert();
                }
                catch
                {
                }
            }

            if (SourceObject.PrimaryCountryInfo != null)
            {
                try
                {
                    details.PrimaryCountryInfo = InstaConvertersFabric.Instance
                        .GetPrimaryCountryInfoConverter(SourceObject.PrimaryCountryInfo)
                        .Convert();
                }
                catch
                {
                }
            }

            return details;
        }
    }
}