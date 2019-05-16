using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaContactUserResponse
    {
        [JsonProperty("user")]
        public InstaUserContactResponse User { get; set; }
    }
}