using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaBlockedUserInfoResponse
    {
        [JsonProperty("block_at")] public long BlockedAt { get; set; }

        [JsonProperty("full_name")] public string FullName { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("user_id")] public long Pk { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicture { get; set; }

        [JsonProperty("username")] public string UserName { get; set; }
    }
}
