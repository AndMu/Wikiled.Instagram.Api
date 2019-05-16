using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaChallengeRequireStepDataEmailVerify
    {
        [JsonProperty("contact_point")]
        public string ContactPoint { get; set; }

        [JsonProperty("form_type")]
        public string FormType { get; set; }

        [JsonProperty("resend_delay")]
        public int ResendDelay { get; set; }

        [JsonProperty("security_code")]
        public string SecurityCode { get; set; }
    }
}