using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsRootResponse
    {
        [JsonProperty("data")] public InstaStatisticsDataResponse Data { get; set; }
    }
}
