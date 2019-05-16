using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaMediaAlbumResponse
    {
        [JsonProperty("client_sidecar_id")]
        public string ClientSidecarId { get; set; }

        [JsonProperty("media")]
        public InstaMediaItemResponse Media { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}