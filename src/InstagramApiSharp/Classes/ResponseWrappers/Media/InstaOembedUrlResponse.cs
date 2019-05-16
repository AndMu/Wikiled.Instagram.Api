using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Media
{
    public class InstaOembedUrlResponse
    {
        [JsonProperty("media_id")] //media_id is enough.
        public string MediaId { get; set; }
    }
}
