using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaAnimatedImageMediaResponse
    {
        [JsonProperty("height")] public string Height { get; set; }

        [JsonProperty("mp4")] public string Mp4 { get; set; }

        [JsonProperty("mp4_size")] public string Mp4Size { get; set; }

        [JsonProperty("size")] public string Size { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("webp")] public string Webp { get; set; }

        [JsonProperty("webp_size")] public string WebpSize { get; set; }

        [JsonProperty("width")] public string Width { get; set; }
    }
}
