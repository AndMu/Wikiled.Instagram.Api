using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsEdgeResponse
    {
        [JsonProperty("node")]
        public InstaMediaShortResponse Node { get; set; }
    }
}