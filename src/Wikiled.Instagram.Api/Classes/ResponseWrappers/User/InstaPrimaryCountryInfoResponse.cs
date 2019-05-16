using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaPrimaryCountryInfoResponse
    {
        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("has_country")]
        public bool? HasCountry { get; set; }

        [JsonProperty("is_visible")]
        public bool? IsVisible { get; set; }
    }
}