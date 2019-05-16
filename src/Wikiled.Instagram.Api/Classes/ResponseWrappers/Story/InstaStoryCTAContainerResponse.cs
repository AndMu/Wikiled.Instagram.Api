using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryCtaContainerResponse
    {
        [JsonProperty("links")]
        public InstaStoryCtaResponse[] Links { get; set; }
    }
}