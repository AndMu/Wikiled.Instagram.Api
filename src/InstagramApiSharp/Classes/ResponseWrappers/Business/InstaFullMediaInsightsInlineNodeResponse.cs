using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsInlineNodeResponse
    {
        [JsonProperty("metrics")] public InstaFullMediaInsightsMetricsResponse Metrics { get; set; }

        [JsonProperty("state")] public string State { get; set; }
    }
}
