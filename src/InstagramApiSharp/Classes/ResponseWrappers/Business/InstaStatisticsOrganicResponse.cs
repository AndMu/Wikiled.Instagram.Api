using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsOrganicResponse
    {
        [JsonProperty("value")] public long? Value { get; set; } = 0;
    }
}
