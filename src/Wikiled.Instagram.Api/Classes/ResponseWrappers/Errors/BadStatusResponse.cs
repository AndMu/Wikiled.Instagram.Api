using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Challenge;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class InstaBadStatusResponse : InstaBaseStatusResponse
    {
        [JsonProperty("challenge")]
        public InstaChallengeLoginInfo Challenge { get; set; }

        [JsonProperty("checkpoint_url")]
        public string CheckPointUrl { get; set; }

        [JsonProperty("error_type")]
        public string ErrorType { get; set; }

        [JsonProperty("feedback_message")]
        public string FeedbackMessage { get; set; }

        [JsonProperty("feedback_title")]
        public string FeedbackTitle { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("payload")]
        public InstaPayloadResponse Payload { get; set; }

        [JsonProperty("spam")]
        public bool Spam { get; set; }
    }
}