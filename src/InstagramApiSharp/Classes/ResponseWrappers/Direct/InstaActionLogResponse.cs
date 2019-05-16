using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaActionLogResponse
    {
        [JsonProperty("description")] public string Description { get; set; }
    }
}
