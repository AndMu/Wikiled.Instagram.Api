using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaSectionMediaResponse
    {
        [JsonProperty("explore_item_info")]
        public InstaSectionMediaExploreItemInfoResponse ExploreItemInfo { get; set; }

        [JsonProperty("feed_type")]
        public string FeedType { get; set; }

        [JsonProperty("layout_content")]
        public InstaSectionMediaLayoutContentResponse LayoutContent { get; set; }

        [JsonProperty("layout_type")]
        public string LayoutType { get; set; }
    }
}