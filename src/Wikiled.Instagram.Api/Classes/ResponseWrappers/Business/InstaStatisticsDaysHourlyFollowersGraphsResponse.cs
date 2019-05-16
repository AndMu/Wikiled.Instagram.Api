using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDaysHourlyFollowersGraphsResponse : InstaStatisticsDataPointsResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}