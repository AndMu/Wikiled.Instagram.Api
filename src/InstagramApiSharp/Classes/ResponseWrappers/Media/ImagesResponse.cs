using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class ImagesResponse
    {
        [JsonProperty("low_resolution")] public ImageResponse LowResolution { get; set; }

        [JsonProperty("standard_resolution")] public ImageResponse StandartResolution { get; set; }

        [JsonProperty("thumbnail")] public ImageResponse Thumbnail { get; set; }
    }
}
