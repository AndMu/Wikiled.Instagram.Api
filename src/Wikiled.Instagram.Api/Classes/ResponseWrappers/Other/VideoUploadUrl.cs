using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaVideoUploadUrl
    {
        [JsonProperty("expires")]
        public double Expires { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}