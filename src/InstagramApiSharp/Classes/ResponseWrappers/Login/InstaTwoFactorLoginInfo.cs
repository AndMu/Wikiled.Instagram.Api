using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    public class InstaTwoFactorLoginInfo
    {
        public static InstaTwoFactorLoginInfo Empty => new InstaTwoFactorLoginInfo();

        [JsonProperty("obfuscated_phone_number")]
        public string ObfuscatedPhoneNumber { get; set; }

        [JsonProperty("phone_verification_settings")]
        public InstaPhoneVerificationSettings PhoneVerificationSettings { get; set; }

        [JsonProperty("show_messenger_code_option")]
        public bool? ShowMessengerCodeOption { get; set; }

        [JsonProperty("two_factor_identifier")]
        public string TwoFactorIdentifier { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }
}
