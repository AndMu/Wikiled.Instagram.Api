using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaPhoneNumberRegistration
    {
        [JsonProperty("error_type")]
        public string ErrorType { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("pn_taken")]
        public bool PnTaken { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}