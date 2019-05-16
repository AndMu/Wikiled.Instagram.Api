using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryCTAContainerResponse
    {
        [JsonProperty("links")] public InstaStoryCTAResponse[] Links { get; set; }
    }
}
