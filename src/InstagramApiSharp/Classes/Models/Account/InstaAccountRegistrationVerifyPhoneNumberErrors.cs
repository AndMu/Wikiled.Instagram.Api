using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class InstaAccountRegistrationVerifyPhoneNumberErrors
    {
        [JsonProperty("nonce")] public string[] Nonce { get; set; }
    }
}
