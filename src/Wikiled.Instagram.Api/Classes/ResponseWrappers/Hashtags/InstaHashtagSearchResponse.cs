using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaHashtagSearchResponse : InstaBaseStatusResponse
    {
        [JsonProperty("has_more")]
        public bool? MoreAvailable { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; }

        [JsonIgnore]
        public List<InstaHashtagResponse> Tags { get; set; } = new List<InstaHashtagResponse>();
    }
}