using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsMetricsResponse
    {
        [JsonProperty("impressions")]
        public InstaStatisticsImpressionsResponse Impressions { get; set; }
    }
}