using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsBusinessProfileResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}