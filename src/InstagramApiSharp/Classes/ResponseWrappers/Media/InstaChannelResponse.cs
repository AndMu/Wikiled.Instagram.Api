using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaChannelResponse
    {
        [JsonProperty("channel_id")] public string ChannelId { get; set; }

        [JsonProperty("channel_type")] public string ChannelType { get; set; }

        [JsonProperty("context")] public string Context { get; set; }

        [JsonProperty("header")] public string Header { get; set; }

        [JsonProperty("media")] public InstaMediaItemResponse Media { get; set; }

        [JsonProperty("title")] public string Title { get; set; }
    }
}
