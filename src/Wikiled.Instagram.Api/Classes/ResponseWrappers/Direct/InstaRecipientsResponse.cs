using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRecipientsResponse : InstaBaseStatusResponse
    {
        [JsonProperty("expires")]
        public long Expires { get; set; }

        [JsonProperty("filtered")]
        public bool Filtered { get; set; }

        [JsonProperty("rank_token")]
        public string RankToken { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }
}