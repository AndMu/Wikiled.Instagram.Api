using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserContainerResponse : InstaDefault
    {
        [JsonProperty("user")] public InstaUserResponse User { get; set; }
    }
}
