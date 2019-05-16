using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsDataResponse
    {
        [JsonProperty("media")] public InstaFullMediaInsightsResponse Media { get; set; }
    }
}
