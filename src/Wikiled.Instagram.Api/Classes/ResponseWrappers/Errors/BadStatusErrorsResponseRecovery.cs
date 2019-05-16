using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Errors
{
    public class InstaBadStatusErrorsResponseRecovery : InstaBaseStatusResponse
    {
        [JsonProperty("errors")]
        public InstaMessageErrorsResponsePhone PhoneNumber { get; set; }
    }
}