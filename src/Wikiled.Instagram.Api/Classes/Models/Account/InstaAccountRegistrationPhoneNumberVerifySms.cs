using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class InstaAccountRegistrationPhoneNumberVerifySms
    {
        [JsonProperty("errors")] public InstaAccountRegistrationVerifyPhoneNumberErrors Errors { get; set; }

        [JsonProperty("error_type")] public string ErrorType { get; set; }

        [JsonProperty("nonce_valid")] public bool NonceValid { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("verified")] public bool Verified { get; set; }
    }
}
