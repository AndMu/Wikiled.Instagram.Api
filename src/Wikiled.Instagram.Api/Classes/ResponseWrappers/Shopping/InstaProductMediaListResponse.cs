using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Shopping
{
    public class InstaProductMediaListResponse : BaseLoadableResponse
    {
        [JsonProperty("items")] public List<InstaMediaItemResponse> Medias { get; set; } = new List<InstaMediaItemResponse>();
    }
}
