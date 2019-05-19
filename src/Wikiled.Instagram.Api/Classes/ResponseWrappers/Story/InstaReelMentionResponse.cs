using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaReelMentionResponse
    {
        [JsonProperty("hashtag")]
        public HashtagResponse Hashtag { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("is_hidden")]
        public int IsHidden { get; set; }

        [JsonProperty("is_pinned")]
        public int IsPinned { get; set; }

        [JsonProperty("rotation")]
        public double Rotation { get; set; }

        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }

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