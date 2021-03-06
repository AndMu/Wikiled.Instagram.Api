﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover
{
    public class InstaDiscoverTopSearchesResponse
    {
        [JsonProperty("clear_client_cache")]
        public bool ClearClientCache { get; set; }

        [JsonProperty("has_more")]
        public bool HasMoreItems { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("list")]
        public List<InstaDiscoverSearchesResponse> TopResults { get; set; }
    }
}