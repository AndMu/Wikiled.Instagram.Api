using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class InstaPlaceListResponse
    {
        [JsonIgnore]
        public List<long> ExcludeList = new List<long>();

        [JsonProperty("has_more")] public bool? HasMore { get; set; }

        [JsonProperty("items")] public List<InstaPlaceResponse> Items { get; set; }

        [JsonProperty("rank_token")] public string RankToken { get; set; }

        [JsonProperty("message")] internal string Message { get; set; }

        [JsonProperty("status")] internal string Status { get; set; }
    }
}
