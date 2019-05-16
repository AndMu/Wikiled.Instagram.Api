using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaSuggestionUserContainerResponse
    {
        [JsonProperty("max_id")] public string MaxId { get; set; }

        [JsonProperty("more_available")] public bool MoreAvailable { get; set; }

        [JsonProperty("new_suggested_users")] public InstaSuggestionResponse NewSuggestedUsers { get; set; } = new InstaSuggestionResponse();

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("suggested_users")] public InstaSuggestionResponse SuggestedUsers { get; set; } = new InstaSuggestionResponse();
    }
}
