using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFriendshipShortStatusListResponse : List<InstaFriendshipShortStatusResponse>
    {
        [JsonProperty("status")] public string Status { get; set; }
    }
}
