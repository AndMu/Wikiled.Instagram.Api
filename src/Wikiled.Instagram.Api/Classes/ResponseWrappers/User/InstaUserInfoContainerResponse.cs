using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserInfoContainerResponse : InstaBaseStatusResponse
    {
        [JsonProperty("user")]
        public InstaUserInfoResponse User { get; set; }
    }
}