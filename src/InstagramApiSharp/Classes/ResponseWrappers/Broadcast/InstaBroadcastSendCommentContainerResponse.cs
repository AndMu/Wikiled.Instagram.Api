using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastSendCommentContainerResponse
    {
        [JsonProperty("comment")] public InstaBroadcastSendCommentResponse Comment { get; set; }

        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
