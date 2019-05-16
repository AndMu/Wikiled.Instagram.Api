using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class ImageResponse
    {
        [JsonProperty("height")] public string Height { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("width")] public string Width { get; set; }
    }
}
