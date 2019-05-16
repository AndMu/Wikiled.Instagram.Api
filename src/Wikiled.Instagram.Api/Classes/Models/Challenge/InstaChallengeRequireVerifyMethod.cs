using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaChallengeRequireVerifyMethod
    {
        [JsonProperty("nonce_code")]
        public string NonceCode { get; set; }

        [JsonProperty("step_data")]
        public InstaChallengeRequireStepData StepData { get; set; }

        [JsonProperty("step_name")]
        public string StepName { get; set; }

        public bool SubmitPhoneRequired => StepName == "submit_phone";

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("message")]
        internal string Message { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}