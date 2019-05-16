using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaUserEditContainer
    {
        [JsonProperty("user")]
        public InstaUserEdit User { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}