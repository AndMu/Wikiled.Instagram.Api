using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountCreationErrors
    {
        [JsonProperty("username")]
        public string[] Username { get; set; }
    }
}