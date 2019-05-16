using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebDataListResponse
    {
        [JsonProperty("cursor")] public string Cursor { get; set; }

        [JsonProperty("data")] public List<InstaWebDataItemResponse> Data { get; set; } = new List<InstaWebDataItemResponse>();

        [JsonProperty("link")] public object Link { get; set; }
    }
}
