using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class ImageThumbnailResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("upload_id")] public string UploadId { get; set; }

        [JsonProperty("xsharing_nonces")] public object XSharingNonces { get; set; }
    }
}
