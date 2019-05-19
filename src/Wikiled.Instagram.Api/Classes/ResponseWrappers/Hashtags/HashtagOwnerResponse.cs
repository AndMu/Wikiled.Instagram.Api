using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class HashtagOwnerResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pk")]
        public string Pk { get; set; }

        [JsonProperty("profile_pic_url")]
        public string ProfilePicUrl { get; set; }

        [JsonProperty("profile_pic_username")]
        public string ProfilePicUsername { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}