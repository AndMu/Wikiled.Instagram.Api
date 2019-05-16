using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverSearchResultResponse
    {
        [JsonProperty("has_more")] public bool? HasMore { get; set; }

        [JsonProperty("num_results")] public int? NumResults { get; set; }

        [JsonProperty("rank_token")] public string RankToken { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("users")] public List<InstaUserResponse> Users { get; set; }
    }
}
