using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Location
{
    public class PlaceResponse
    {
        [JsonProperty("location")]
        public PlaceShortResponse Location { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}