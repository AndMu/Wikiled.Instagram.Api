using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountTwoFactorSms
    {
        [JsonProperty("obfuscated_phone_number")]
        public string ObfuscatedPhoneNumber { get; set; }

        [JsonProperty("phone_verification_settings")]
        public InstaAccountPhoneVerificationSettings PhoneVerificationSettings { get; set; }

        [JsonProperty("status")] internal string Status { get; set; }
    }
}
