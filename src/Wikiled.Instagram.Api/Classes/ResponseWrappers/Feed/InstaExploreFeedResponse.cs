using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed
{
    public class InstaExploreFeedResponse : BaseLoadableResponse
    {
        [JsonIgnore] public InstaExploreItemsResponse Items { get; set; } = new InstaExploreItemsResponse();

        [JsonProperty("max_id")] public string MaxId { get; set; }
    }
}
