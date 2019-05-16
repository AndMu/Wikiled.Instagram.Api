using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountCheck
    {
        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("error_type")]
        internal string ErrorType { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}