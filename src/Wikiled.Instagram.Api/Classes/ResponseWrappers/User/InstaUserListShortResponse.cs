using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserListShortResponse : InstaBaseStatusResponse
    {
        [JsonProperty("big_list")]
        public bool IsBigList { get; set; }

        [JsonProperty("users")]
        public List<InstaUserShortResponse> Items { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }
    }
}