using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaPersistentSectionResponse
    {
        [JsonProperty("layout_content")]
        public InstaPersistentSectionLayoutContentResponse LayoutContent { get; set; }

        [JsonProperty("layout_type")]
        public string LayoutType { get; set; }
    }
}