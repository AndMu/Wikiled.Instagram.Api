using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataPointsNodeResponse : InstaStatisticsDataPointsResponse
    {
        [JsonProperty("graph_name")] public string GraphName { get; set; }
    }
}
