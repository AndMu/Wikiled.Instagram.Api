using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaCaptionResponse : BaseStatusResponse
    {
        [JsonProperty("content_type")] public string ContentType { get; set; }

        [JsonProperty("created_at")] public string CreatedAtUnixLike { get; set; }

        [JsonProperty("created_at_utc")] public string CreatedAtUtcUnixLike { get; set; }

        [JsonProperty("media_id")] public string MediaId { get; set; }

        [JsonProperty("pk")] public string Pk { get; set; }

        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }

        [JsonProperty("user_id")] public long UserId { get; set; }
    }
}
