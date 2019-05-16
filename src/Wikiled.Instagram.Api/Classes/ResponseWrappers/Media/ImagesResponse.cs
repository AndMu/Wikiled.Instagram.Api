using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaImagesResponse
    {
        [JsonProperty("low_resolution")]
        public InstaImageResponse LowResolution { get; set; }

        [JsonProperty("standard_resolution")]
        public InstaImageResponse StandartResolution { get; set; }

        [JsonProperty("thumbnail")]
        public InstaImageResponse Thumbnail { get; set; }
    }
}