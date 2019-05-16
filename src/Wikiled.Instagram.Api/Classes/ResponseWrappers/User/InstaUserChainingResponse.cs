using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserChainingResponse : InstaUserShortResponse
    {
        [JsonProperty("chaining_info")]
        public InstaUserChainingInfoResponse ChainingInfo { get; set; }

        [JsonProperty("profile_chaining_secondary_label")]
        public string ProfileChainingSecondaryLabel { get; set; }
    }
}