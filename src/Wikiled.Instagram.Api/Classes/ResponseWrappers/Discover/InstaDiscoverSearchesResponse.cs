using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverSearchesResponse
    {
        [JsonProperty("client_time")]
        public int? ClientTime { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("user")]
        public InstaUserResponse User { get; set; }
    }
}