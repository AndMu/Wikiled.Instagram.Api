using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaRegistrationSuggestion
    {
        [JsonProperty("prototype")] public string Prototype { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }
}
