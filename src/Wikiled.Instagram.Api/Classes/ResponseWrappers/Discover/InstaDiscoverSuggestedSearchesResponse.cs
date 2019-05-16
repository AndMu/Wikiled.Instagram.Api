using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverSuggestedSearchesResponse
    {
        [JsonProperty("rank_token")] public string RankToken { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("suggested")] public List<InstaDiscoverSearchesResponse> Suggested { get; set; }
    }
}
