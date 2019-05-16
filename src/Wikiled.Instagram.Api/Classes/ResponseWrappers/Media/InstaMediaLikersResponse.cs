using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaLikersResponse : BadStatusResponse
    {
        [JsonProperty("users")] public List<InstaUserShortResponse> Users { get; set; }

        [JsonProperty("user_count")] public int UsersCount { get; set; }
    }
}
