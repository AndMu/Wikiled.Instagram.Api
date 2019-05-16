using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverRecentSearchesResponse
    {
        [JsonProperty("recent")] public List<InstaDiscoverSearchesResponse> Recent { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
