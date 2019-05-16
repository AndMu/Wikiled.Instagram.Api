using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaRegistrationSuggestionsList
    {
        [JsonProperty("suggestions")]
        public InstaRegistrationSuggestion[] Suggestions { get; set; }
    }
}