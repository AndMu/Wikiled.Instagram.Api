using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class SectionMediaListResponse
    {
        [JsonProperty("auto_load_more_enabled")]
        public bool? AutoLoadMoreEnabled { get; set; }

        [JsonProperty("more_available")]
        public bool MoreAvailable { get; set; }

        [JsonProperty("next_max_id")]
        public string NextMaxId { get; set; }

        [JsonProperty("next_media_ids")]
        public List<long> NextMediaIds { get; set; }

        [JsonProperty("next_page")]
        public int? NextPage { get; set; }

        [JsonProperty("persistent_sections")]
        public List<PersistentSectionResponse> PersistentSections { get; set; } =
            new List<PersistentSectionResponse>();

        [JsonProperty("sections")]
        public List<SectionMediaResponse> Sections { get; set; } = new List<SectionMediaResponse>();

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}