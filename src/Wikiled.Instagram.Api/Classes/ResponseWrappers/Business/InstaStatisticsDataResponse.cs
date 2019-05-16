using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsDataResponse
    {
        [JsonProperty("user")]
        public InstaStatisticsUserDataResponse User { get; set; }
    }
}