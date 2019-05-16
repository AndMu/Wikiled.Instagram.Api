using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserContextResponse
    {
        [JsonProperty("end")] public int End { get; set; }

        [JsonProperty("start")] public int Start { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }
}
