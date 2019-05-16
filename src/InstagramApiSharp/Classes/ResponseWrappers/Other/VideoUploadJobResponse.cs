using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class VideoUploadJobResponse
    {
        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("upload_id")] public string UploadId { get; set; }

        [JsonProperty("video_upload_urls")] public List<VideoUploadUrl> VideoUploadUrls { get; set; }

        [JsonProperty("xsharing_nonces")] public object XSharingNonces { get; set; }
    }
}
