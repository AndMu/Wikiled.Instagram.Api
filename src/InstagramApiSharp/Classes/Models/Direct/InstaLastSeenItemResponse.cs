using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaLastSeenItemResponse
    {
        [JsonProperty("item_id")] public string ItemId { get; set; }

        [JsonProperty("timestamp")] internal string TimestampPrivate { get; set; }
    }
}
