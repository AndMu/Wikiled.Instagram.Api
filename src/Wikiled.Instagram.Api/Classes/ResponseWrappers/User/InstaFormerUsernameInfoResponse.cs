using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFormerUsernameInfoResponse
    {
        [JsonProperty("has_former_usernames")]
        public bool? HasFormerUsernames { get; set; }
    }
}