using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class SectionMediaResponse
    {
        [JsonProperty("explore_item_info")]
        public SectionMediaExploreItemInfoResponse ExploreItemInfo { get; set; }

        [JsonProperty("feed_type")]
        public string FeedType { get; set; }

        [JsonProperty("layout_content")]
        public SectionMediaLayoutContentResponse LayoutContent { get; set; }

        [JsonProperty("layout_type")]
        public string LayoutType { get; set; }
    }
}