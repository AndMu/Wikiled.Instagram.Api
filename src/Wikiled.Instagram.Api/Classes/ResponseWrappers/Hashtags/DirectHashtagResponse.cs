using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class DirectHashtagResponse
    {
        [JsonProperty("media")]
        public InstaMediaItemResponse Media { get; set; }

        [JsonProperty("media_count")]
        public long MediaCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}