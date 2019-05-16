using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaWebLinkResponse
    {
        [JsonProperty("link_context")]
        public InstaWebLinkContextResponse LinkContext { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}