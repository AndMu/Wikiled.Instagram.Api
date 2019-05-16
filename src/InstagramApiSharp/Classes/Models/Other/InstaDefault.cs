using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Other
{
    public class InstaDefault
    {
        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
