using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaAdsInfoResponse
    {
        [JsonProperty("ads_url")]
        public string AdsUrl { get; set; }

        [JsonProperty("has_ads")]
        public bool? HasAds { get; set; }
    }
}