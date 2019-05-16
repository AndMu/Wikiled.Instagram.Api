using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountConfirmEmail
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("is_email_legit")]
        public bool IsEmailLegit { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}