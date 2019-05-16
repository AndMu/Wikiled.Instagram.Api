using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataPointsDailyNodesResponse
    {
        [JsonProperty("total_count_graph")]
        public InstaStatisticsDataPointsNodeResponse TotalCountGraph { get; set; }
    }
}