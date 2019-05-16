using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaBusinessUserContainerResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("user")] public InstaBusinessUserResponse User { get; set; }
    }
}
