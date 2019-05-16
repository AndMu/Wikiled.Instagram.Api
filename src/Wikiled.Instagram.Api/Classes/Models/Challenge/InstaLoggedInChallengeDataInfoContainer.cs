using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    internal class InstaLoggedInChallengeDataInfoContainer
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("step_data")]
        public InstaLoggedInChallengeDataInfo StepData { get; set; }

        [JsonProperty("step_name")]
        public string StepName { get; set; }
    }
}