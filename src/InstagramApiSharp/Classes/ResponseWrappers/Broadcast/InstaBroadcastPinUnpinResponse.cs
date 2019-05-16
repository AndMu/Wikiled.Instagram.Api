using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastPinUnpinResponse
    {
        [JsonProperty("comment_id")] public long CommentId { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
