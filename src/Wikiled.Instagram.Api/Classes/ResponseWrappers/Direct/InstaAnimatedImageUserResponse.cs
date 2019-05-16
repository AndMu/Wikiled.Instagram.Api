using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaAnimatedImageUserResponse
    {
        [JsonProperty("is_verified")] public bool IsVerified { get; set; }

        [JsonProperty("username")] public string Username { get; set; }
    }
}
