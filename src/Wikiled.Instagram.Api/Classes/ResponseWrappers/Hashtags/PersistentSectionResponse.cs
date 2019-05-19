using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class PersistentSectionResponse
    {
        [JsonProperty("layout_content")]
        public PersistentSectionLayoutContentResponse LayoutContent { get; set; }

        [JsonProperty("layout_type")]
        public string LayoutType { get; set; }
    }
}