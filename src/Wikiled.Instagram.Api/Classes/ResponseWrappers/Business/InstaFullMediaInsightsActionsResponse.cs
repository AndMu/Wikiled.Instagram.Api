using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsActionsResponse : InstaFullMediaInsightsNodeResponse
    {
        [JsonProperty("value")] public int? Value { get; set; }
    }
}
