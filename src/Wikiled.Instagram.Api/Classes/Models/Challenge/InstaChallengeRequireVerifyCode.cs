using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaChallengeRequireVerifyCode
    {
        [JsonIgnore]
        public bool IsLoggedIn => LoggedInUser != null || Status.ToLower() == "ok";

        [JsonProperty("logged_in_user")]
        public /*InstaUserInfoResponse*/ InstaUserShortResponse LoggedInUser { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("action")]
        internal string Action { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}