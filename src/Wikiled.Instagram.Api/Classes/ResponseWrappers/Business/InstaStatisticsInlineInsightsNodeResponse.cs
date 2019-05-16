using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsInlineInsightsNodeResponse
    {
        [JsonProperty("metrics")] public InstaStatisticsMetricsResponse Metrics { get; set; }

        [JsonProperty("state")] public string State { get; set; }
    }
}
