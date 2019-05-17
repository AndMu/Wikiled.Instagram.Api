using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class AccountSendSms
    {
        [JsonProperty("phone_number_valid")]
        public bool PhoneNumberValid { get; set; }

        [JsonProperty("phone_verification_settings")]
        public InstaAccountPhoneVerificationSettings PhoneVerificationSettings { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}