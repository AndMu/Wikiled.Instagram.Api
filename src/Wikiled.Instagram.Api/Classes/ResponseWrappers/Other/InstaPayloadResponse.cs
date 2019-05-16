using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaPayloadResponse
    {
        [JsonProperty("client_context")]
        public string ClientContext { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}