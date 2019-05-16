using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountBesties
    {
        [JsonProperty("big_list")] public bool BigList { get; set; }

        [JsonProperty("page_size")] public int PageSize { get; set; }

        [JsonProperty("users")] public List<InstaUserResponse> Users { get; set; }
    }
}
