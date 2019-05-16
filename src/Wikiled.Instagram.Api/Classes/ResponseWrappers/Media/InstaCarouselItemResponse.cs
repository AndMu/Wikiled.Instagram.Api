using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaCarouselItemResponse
    {
        [JsonProperty("carousel_parent_id")]
        public string CarouselParentId { get; set; }

        [JsonProperty("original_height")]
        public string Height { get; set; }

        [JsonProperty("image_versions2")]
        public InstaImageCandidatesResponse Images { get; set; }

        [JsonProperty("id")]
        public string Identifier { get; set; }

        [JsonProperty("media_type")]
        public InstaMediaType MediaType { get; set; }

        [JsonProperty("pk")]
        public string Pk { get; set; }

        [JsonProperty("usertags")]
        public InstaUserTagListResponse UserTagList { get; set; }

        [JsonProperty("video_versions")]
        public List<InstaVideoResponse> Videos { get; set; }

        [JsonProperty("original_width")]
        public string Width { get; set; }
    }
}