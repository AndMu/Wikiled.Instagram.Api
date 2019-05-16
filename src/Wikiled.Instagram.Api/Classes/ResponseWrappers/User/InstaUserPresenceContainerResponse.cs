using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserPresenceContainerResponse
    {
        [JsonIgnore] public List<InstaUserPresenceResponse> Items { get; set; } = new List<InstaUserPresenceResponse>();

        [JsonProperty("status")] public string Status { get; set; }
    }
}
