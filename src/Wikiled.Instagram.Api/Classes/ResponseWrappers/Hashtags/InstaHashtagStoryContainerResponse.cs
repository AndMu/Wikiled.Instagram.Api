using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags
{
    public class InstaHashtagStoryContainerResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("story")]
        public InstaHashtagStoryResponse Story { get; set; }
    }
}