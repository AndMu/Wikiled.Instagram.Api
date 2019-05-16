using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaPersistentSectionLayoutContentResponse
    {
        [JsonProperty("related")]
        public List<InstaRelatedHashtagResponse> Related { get; set; }

        [JsonProperty("related_style")]
        public string RelatedTtyle { get; set; }
    }
}