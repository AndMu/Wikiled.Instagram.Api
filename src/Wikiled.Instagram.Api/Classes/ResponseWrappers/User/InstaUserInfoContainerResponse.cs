using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserInfoContainerResponse : BaseStatusResponse
    {
        [JsonProperty("user")] public InstaUserInfoResponse User { get; set; }
    }
}
