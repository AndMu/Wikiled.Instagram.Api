using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class InstaPlaceResponse
    {
        [JsonProperty("location")] public InstaPlaceShortResponse Location { get; set; }

        [JsonProperty("subtitle")] public string Subtitle { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        // always empty
        //[JsonProperty("media_bundles")] public object MediaBundles { get; set; }
        // always empty
        //[JsonProperty("header_media")] public object HeaderMedia { get; set; }
    }
}
