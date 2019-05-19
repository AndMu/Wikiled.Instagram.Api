using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class PersistentSectionLayoutContentResponse
    {
        [JsonProperty("related")]
        public List<RelatedHashtagResponse> Related { get; set; }

        [JsonProperty("related_style")]
        public string RelatedTtyle { get; set; }
    }
}