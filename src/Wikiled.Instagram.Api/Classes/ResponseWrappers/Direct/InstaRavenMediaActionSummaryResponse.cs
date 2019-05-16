using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRavenMediaActionSummaryResponse
    {
        [JsonProperty("count")] public int Count { get; set; }

        [JsonProperty("timestamp")] public string TimeStamp { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }
}
