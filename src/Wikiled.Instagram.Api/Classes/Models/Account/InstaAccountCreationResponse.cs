using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class InstaAccountCreationResponse : InstaAccountCreation
    {
        [JsonProperty("errors")]
        public InstaAccountCreationErrors Errors { get; set; }

        [JsonProperty("error_type")]
        public string ErrorType { get; set; }
    }
}