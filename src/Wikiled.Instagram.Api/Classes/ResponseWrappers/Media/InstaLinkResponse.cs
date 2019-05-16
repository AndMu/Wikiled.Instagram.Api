using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaLinkResponse
    {
        [JsonProperty("end")] public string End { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("start")] public string Start { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }
}
