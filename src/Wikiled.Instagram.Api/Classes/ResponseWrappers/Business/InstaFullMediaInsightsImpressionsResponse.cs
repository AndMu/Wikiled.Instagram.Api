using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsImpressionsResponse
    {
        [JsonProperty("surfaces")]
        public InstaFullMediaInsightsNodeResponse Surfaces { get; set; }

        [JsonProperty("value")]
        public int? Value { get; set; }
    }
}