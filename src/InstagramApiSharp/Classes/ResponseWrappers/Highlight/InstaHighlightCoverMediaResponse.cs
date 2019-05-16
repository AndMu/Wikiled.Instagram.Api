using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightCoverMediaResponse
    {
        [JsonProperty("cropped_image_version")]
        public ImageResponse CroppedImageVersion { get; set; }

        [JsonProperty("crop_rect")] public float[] CropRect { get; set; }

        [JsonProperty("full_image_version")] public ImageResponse FullImageVersion { get; set; }

        [JsonProperty("media_id")] public string MediaId { get; set; }
    }
}
