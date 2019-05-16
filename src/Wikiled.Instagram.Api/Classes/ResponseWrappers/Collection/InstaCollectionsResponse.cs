using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection
{
    public class InstaCollectionsResponse : InstaBaseLoadableResponse
    {
        [JsonProperty("items")]
        public List<InstaCollectionItemResponse> Items { get; set; }
    }
}