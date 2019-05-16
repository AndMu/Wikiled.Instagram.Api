using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaContactUserListResponse
    {
        [JsonProperty("items")]
        public List<InstaContactUserResponse> Items { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}