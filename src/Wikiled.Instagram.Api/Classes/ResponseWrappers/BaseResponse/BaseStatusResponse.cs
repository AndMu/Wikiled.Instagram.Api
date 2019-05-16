using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse
{
    public class InstaBaseStatusResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        public bool IsFail()
        {
            return !string.IsNullOrEmpty(Status) && Status.ToLower() == "fail";
        }

        public bool IsOk()
        {
            return !string.IsNullOrEmpty(Status) && Status.ToLower() == "ok";
        }
    }
}