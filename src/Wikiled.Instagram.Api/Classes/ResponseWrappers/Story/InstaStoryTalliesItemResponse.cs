using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryTalliesItemResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("font_size")]
        public float FontSize { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}