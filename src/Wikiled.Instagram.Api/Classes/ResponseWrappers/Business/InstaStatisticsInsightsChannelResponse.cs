using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsInsightsChannelResponse
    {
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tips")]
        public object[] Tips { get; set; }

        [JsonProperty("unseen_count")]
        public int? UnseenCount { get; set; } = 0;
    }
}