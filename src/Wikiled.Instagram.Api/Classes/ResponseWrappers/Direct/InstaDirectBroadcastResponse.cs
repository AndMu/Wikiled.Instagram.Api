using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaDirectBroadcastResponse
    {
        [JsonProperty("broadcast")] public InstaBroadcast Broadcast { get; set; }

        [JsonProperty("is_linked")] public bool? IsLinked { get; set; }

        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("title")] public string Title { get; set; }
    }
}
