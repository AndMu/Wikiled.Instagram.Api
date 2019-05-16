using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaBlockedUsersResponse : InstaDefault
    {
        [JsonProperty("big_list")]
        public bool? BigList { get; set; }

        [JsonProperty("blocked_list")]
        public List<InstaBlockedUserInfoResponse> BlockedList { get; set; }

        [JsonProperty("max_id")]
        public string MaxId { get; set; }

        [JsonProperty("page_size")]
        public int? PageSize { get; set; }
    }
}