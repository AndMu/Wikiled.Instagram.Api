using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaRecentActivityResponse : InstaBaseLoadableResponse
    {
        public bool IsOwnActivity { get; set; } = false;

        [JsonProperty("stories")]
        public List<InstaRecentActivityFeedResponse> Stories { get; set; }
            = new List<InstaRecentActivityFeedResponse>();
    }
}