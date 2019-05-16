using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaFelixShareResponse
    {
        [JsonProperty("video")] public InstaMediaItemResponse Video { get; set; }
    }
}
