using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaActivityMediaResponse
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("image")] public string Image { get; set; }
    }
}
