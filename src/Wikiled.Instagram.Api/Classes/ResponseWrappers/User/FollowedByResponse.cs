using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFollowedByResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}