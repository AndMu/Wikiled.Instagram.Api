using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaAccountDetailsResponse
    {
        [JsonProperty("ads_info")]
        public InstaAdsInfoResponse AdsInfo { get; set; }

        [JsonProperty("date_joined")]
        public int? DateJoined { get; set; }

        [JsonProperty("former_username_info")]
        public InstaFormerUsernameInfoResponse FormerUsernameInfo { get; set; }

        [JsonProperty("primary_country_info")]
        public InstaPrimaryCountryInfoResponse PrimaryCountryInfo { get; set; }

        [JsonProperty("shared_follower_accounts_info")]
        public InstaSharedFollowerAccountsInfoResponse SharedFollowerAccountsInfo { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}