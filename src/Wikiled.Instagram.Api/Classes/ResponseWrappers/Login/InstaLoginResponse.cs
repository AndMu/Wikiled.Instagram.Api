using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    public class InstaLoginResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("logged_in_user")] public InstaUserShortResponse User { get; set; }
    }
}
