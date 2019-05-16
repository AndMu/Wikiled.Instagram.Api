using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class InstaBadStatusErrorsResponse : InstaBaseStatusResponse
    {
        [JsonProperty("message")]
        public InstaMessageErrorsResponse Message { get; set; }
    }
}