using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaPermalinkResponse : InstaBaseStatusResponse
    {
        [JsonProperty("permalink")]
        public string Permalink { get; set; }
    }
}