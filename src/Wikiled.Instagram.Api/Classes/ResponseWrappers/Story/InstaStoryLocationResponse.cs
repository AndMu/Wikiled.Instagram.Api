using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryLocationResponse
    {
        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("is_hidden")]
        public double IsHidden { get; set; }

        [JsonProperty("is_pinned")]
        public double IsPinned { get; set; }

        [JsonProperty("location")]
        public InstaPlaceShortResponse Location { get; set; }

        [JsonProperty("rotation")]
        public double Rotation { get; set; }

        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }
    }
}