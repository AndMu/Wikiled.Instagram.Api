using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class DeleteResponse : BaseStatusResponse
    {
        [JsonProperty("did_delete")] public bool IsDeleted { get; set; }
    }
}
