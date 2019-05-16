using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class FollowedByResponse
    {
        [JsonProperty("count")] public int Count { get; set; }
    }
}
