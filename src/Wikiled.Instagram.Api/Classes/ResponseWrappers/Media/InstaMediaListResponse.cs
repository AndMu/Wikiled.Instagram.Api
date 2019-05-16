using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaListResponse : InstaBaseLoadableResponse
    {
        [JsonProperty("items")]
        public List<InstaMediaItemResponse> Medias { get; set; } = new List<InstaMediaItemResponse>();

        public List<InstaStoryResponse> Stories { get; set; } = new List<InstaStoryResponse>();
    }
}