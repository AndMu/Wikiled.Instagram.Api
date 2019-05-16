using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaInboxMediaResponse
    {
        [JsonProperty("image_versions2")] public InstaImageCandidatesResponse ImageCandidates { get; set; }

        [JsonProperty("media_type")] public InstaMediaType MediaType { get; set; }

        [JsonProperty("original_height")] public long OriginalHeight { get; set; }

        [JsonProperty("original_width")] public long OriginalWidth { get; set; }

        [JsonProperty("video_versions")] public List<InstaVideoResponse> Videos { get; set; }
    }
}
