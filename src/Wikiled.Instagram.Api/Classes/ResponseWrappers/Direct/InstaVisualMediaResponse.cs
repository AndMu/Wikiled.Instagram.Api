using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaVisualMediaResponse
    {
        [JsonProperty("original_height")]
        public int? Height { get; set; }

        [JsonProperty("image_versions2")]
        public InstaImageCandidatesResponse Images { get; set; }

        [JsonProperty("id")]
        public string Identifier { get; set; }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("media_type")]
        public InstaMediaType MediaType { get; set; }

        [JsonProperty("organic_tracking_token")]
        public string TrackingToken { get; set; }

        [JsonProperty("url_expire_at_secs")]
        public long? UrlExpireAtSecs { get; set; }

        [JsonProperty("video_versions")]
        public List<InstaVideoResponse> Videos { get; set; }

        [JsonProperty("original_width")]
        public int? Width { get; set; }
    }
}