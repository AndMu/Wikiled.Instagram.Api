using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebDataResponse
    {
        [JsonProperty("cursor")]
        public string Cursor { get; set; }

        [JsonProperty("data")]
        public InstaWebDataItemResponse Data { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }
    }
}