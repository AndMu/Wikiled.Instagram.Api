using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaDeleteResponse : InstaBaseStatusResponse
    {
        [JsonProperty("did_delete")]
        public bool IsDeleted { get; set; }
    }
}