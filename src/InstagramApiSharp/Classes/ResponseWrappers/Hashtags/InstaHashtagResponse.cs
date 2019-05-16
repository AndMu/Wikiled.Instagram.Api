using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaHashtagResponse
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("media_count")] public long MediaCount { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("profile_pic_url")] public string ProfilePicUrl { get; set; }
    }
}
