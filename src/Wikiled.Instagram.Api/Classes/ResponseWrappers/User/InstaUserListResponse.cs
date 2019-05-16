using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserListResponse : BaseStatusResponse
    {
        [JsonProperty("big_list")] public bool IsBigList { get; set; }

        [JsonProperty("users")] public List<InstaUserResponse> Items { get; set; }

        [JsonProperty("next_max_id")] public string NextMaxId { get; set; }

        [JsonProperty("page_size")] public int PageSize { get; set; }
    }
}
