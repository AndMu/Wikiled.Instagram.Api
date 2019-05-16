using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsImageResponse
    {
        [JsonProperty("uri")] public string Uri { get; set; }
    }
}
