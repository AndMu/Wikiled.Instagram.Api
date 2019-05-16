using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed
{
    public class InstaExploreFeedResponse : InstaBaseLoadableResponse
    {
        [JsonIgnore]
        public InstaExploreItemsResponse Items { get; set; } = new InstaExploreItemsResponse();

        [JsonProperty("max_id")]
        public string MaxId { get; set; }
    }
}