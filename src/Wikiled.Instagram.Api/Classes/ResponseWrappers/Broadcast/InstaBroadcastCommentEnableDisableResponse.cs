using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastCommentEnableDisableResponse
    {
        [JsonProperty("comment_muted")] public int CommentMuted { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
