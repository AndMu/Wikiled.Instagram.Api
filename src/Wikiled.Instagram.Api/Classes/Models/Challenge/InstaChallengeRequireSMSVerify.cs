using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaChallengeRequireSmsVerify
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("nonce_code")]
        public string NonceCode { get; set; }

        [JsonProperty("step_data")]
        public InstaChallengeRequireStepDataSmsVerify StepData { get; set; }

        [JsonProperty("step_name")]
        public string StepName { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}