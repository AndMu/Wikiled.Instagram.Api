using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Collection
{
    public class InstaCollectionsResponse : BaseLoadableResponse
    {
        [JsonProperty("items")] public List<InstaCollectionItemResponse> Items { get; set; }
    }
}
