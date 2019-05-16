using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebContainerResponse
    {
        [JsonProperty("config")]
        public InstaWebConfigResponse Config { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("entry_data")]
        public InstaWebEntryDataResponse Entry { get; set; }

        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}