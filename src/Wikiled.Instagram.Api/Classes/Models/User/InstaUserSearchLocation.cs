using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserSearchLocation
    {
        [JsonProperty("clear_client_cache")]
        public bool? ClearClientCache { get; set; }

        [JsonProperty("has_more")]
        public bool? HasMore { get; set; }

        [JsonProperty("list")]
        public List<InstaUserSearchLocationList> Items { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}