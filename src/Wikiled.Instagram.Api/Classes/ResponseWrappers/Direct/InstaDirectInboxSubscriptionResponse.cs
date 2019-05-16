using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaDirectInboxSubscriptionResponse
    {
        [JsonProperty("auth")] public string Auth { get; set; }

        [JsonProperty("sequence")] public string Sequence { get; set; }

        [JsonProperty("topic")] public string Topic { get; set; }

        [JsonProperty("url")] public string Url { get; set; }
    }
}
