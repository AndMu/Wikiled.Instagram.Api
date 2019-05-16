using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightShortResponse
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("latest_reel_media")] public int LatestReelMedia { get; set; }

        [JsonProperty("media_count")] public int MediaCount { get; set; }

        [JsonProperty("reel_type")] public string ReelType { get; set; }

        [JsonProperty("timestamp")] public long? Timestamp { get; set; }
    }
}
