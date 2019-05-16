using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class InstaMessageErrorsResponseRecoveryEmail : InstaBaseStatusResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}