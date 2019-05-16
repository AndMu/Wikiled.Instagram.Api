using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountBesties
    {
        [JsonProperty("big_list")]
        public bool BigList { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("users")]
        public List<InstaUserResponse> Users { get; set; }
    }
}