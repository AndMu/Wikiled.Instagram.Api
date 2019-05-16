using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsRootResponse
    {
        [JsonProperty("data")] public InstaFullMediaInsightsDataResponse Data { get; set; }
    }
}
