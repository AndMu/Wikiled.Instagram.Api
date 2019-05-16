using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Other
{
    public class InstaRequestDownloadData
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}