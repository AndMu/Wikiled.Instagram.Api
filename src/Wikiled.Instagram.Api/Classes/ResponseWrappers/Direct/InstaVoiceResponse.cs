using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaVoiceResponse
    {
        [JsonProperty("audio")]
        public InstaAudioResponse Audio { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("media_type")]
        public int MediaType { get; set; }

        [JsonProperty("organic_tracking_token")]
        public string OrganicTrackingToken { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("user")]
        public InstaVoiceUserResponse User { get; set; }
    }
}