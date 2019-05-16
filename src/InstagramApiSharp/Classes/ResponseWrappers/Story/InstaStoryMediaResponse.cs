using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryMediaResponse
    {
        [JsonProperty("media")] public InstaStoryItemResponse Media { get; set; }
    }
}
