using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserShortResponse : InstaBaseStatusResponse
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }

        [JsonProperty("profile_pic_url")]
        public string ProfilePicture { get; set; }

        [JsonProperty("profile_pic_id")]
        public string ProfilePictureId { get; set; } = "unknown";

        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}