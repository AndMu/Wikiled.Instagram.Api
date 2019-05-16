using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBiography
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("user")] public InstaBiographyUser User { get; set; }
    }
}
