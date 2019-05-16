using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed
{
    public class InstaTopicalExploreFeedResponse : BaseLoadableResponse
    {
        [JsonIgnore] public InstaChannelResponse Channel { get; set; }

        [JsonProperty("clusters")] public List<InstaTopicalExploreClusterResponse> Clusters { get; set; } = new List<InstaTopicalExploreClusterResponse>();

        [JsonProperty("has_shopping_channel_content")]
        public bool? HasShoppingChannelContent { get; set; }

        [JsonProperty("max_id")] public string MaxId { get; set; }

        [JsonIgnore] public List<InstaMediaItemResponse> Medias { get; set; } = new List<InstaMediaItemResponse>();

        [JsonIgnore] public List<InstaTVChannelResponse> TVChannels { get; set; } = new List<InstaTVChannelResponse>();
    }
}
