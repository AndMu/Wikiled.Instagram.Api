using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaSharing
    {
        [JsonProperty("action")] public string Action { get; set; }

        [JsonProperty("payload")] public InstaSharingPayload[] Payload { get; set; }

        [JsonProperty("status_code")] public string StatusCode { get; set; }

        [JsonProperty("status")] internal string Status { get; set; }
    }
}
