using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaSuggestionUserDetailContainerResponse : InstaDefault
    {
        [JsonProperty("items")]
        public InstaSuggestionItemListResponse Items { get; set; } = new InstaSuggestionItemListResponse();
    }
}