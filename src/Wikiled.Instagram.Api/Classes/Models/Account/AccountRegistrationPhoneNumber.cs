using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class AccountRegistrationPhoneNumber
    {
        [JsonProperty("gdpr_required")]
        public bool GdprRequired { get; set; }

        [JsonIgnore]
        public bool Succeed => Status.ToLower() == "ok" ? true : false;

        [JsonProperty("tos_version")]
        public string TosVersion { get; set; }

        [JsonProperty("error_source")]
        internal string ErrorSource { get; set; }

        [JsonProperty("error_type")]
        internal string ErrorType { get; set; }

        [JsonProperty("message")]
        internal InstaAccountRegistrationPhoneNumberMessage Message { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}