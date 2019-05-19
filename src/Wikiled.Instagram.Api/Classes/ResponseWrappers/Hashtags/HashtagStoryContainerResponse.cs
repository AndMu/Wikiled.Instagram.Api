using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class HashtagStoryContainerResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("story")]
        public HashtagStoryResponse Story { get; set; }
    }
}