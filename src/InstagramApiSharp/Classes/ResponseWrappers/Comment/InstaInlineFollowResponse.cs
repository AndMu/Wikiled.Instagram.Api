using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaInlineFollowResponse
    {
        [JsonProperty("following")] public bool IsFollowing { get; set; }

        [JsonProperty("outgoing_request")] public bool IsOutgoingRequest { get; set; }

        [JsonProperty("user_info")] public InstaUserShortResponse UserInfo { get; set; }
    }
}
