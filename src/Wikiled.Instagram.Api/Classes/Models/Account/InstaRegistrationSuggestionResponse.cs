using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaRegistrationSuggestionResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("suggestions_with_metadata")]
        public InstaRegistrationSuggestionsList SuggestionsWithMetadata { get; set; }
    }
}
