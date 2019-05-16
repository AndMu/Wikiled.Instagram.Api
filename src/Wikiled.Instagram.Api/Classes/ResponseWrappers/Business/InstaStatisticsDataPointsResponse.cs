using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataPointsResponse
    {
        [JsonProperty("data_points")]
        public InstaStatisticsDataPointItemResponse[] DataPoints { get; set; }
    }
}