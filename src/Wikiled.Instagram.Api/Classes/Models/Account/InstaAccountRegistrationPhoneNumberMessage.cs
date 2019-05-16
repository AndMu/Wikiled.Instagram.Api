using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class InstaAccountRegistrationPhoneNumberMessage
    {
        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}