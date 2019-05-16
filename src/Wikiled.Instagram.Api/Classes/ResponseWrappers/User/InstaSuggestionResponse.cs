using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaSuggestionResponse
    {
        [JsonProperty("suggestions")]
        public InstaSuggestionItemListResponse Suggestions { get; set; } = new InstaSuggestionItemListResponse();
    }
}