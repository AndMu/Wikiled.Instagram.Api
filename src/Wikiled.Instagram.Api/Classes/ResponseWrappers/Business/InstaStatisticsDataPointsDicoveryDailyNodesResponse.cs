using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataPointsDicoveryDailyNodesResponse
    {
        [JsonProperty("nodes")]
        public InstaStatisticsDataPointsNodeResponse[] Nodes { get; set; }
    }
}