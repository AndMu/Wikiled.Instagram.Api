using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class AccountVerifySms
    {
        [JsonProperty("errors")]
        public InstaAccountVerifySmsErrors Errors { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("error_type")]
        internal string ErrorType { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}