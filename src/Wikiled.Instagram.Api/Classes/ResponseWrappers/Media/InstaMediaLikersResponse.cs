using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaLikersResponse : InstaBadStatusResponse
    {
        [JsonProperty("users")]
        public List<InstaUserShortResponse> Users { get; set; }

        [JsonProperty("user_count")]
        public int UsersCount { get; set; }
    }
}