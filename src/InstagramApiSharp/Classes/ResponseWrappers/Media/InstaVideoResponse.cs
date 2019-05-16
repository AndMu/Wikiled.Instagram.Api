using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaVideoResponse
    {
        [JsonProperty("height")] public string Height { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("type")] public int Type { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("width")] public string Width { get; set; }
    }
}
