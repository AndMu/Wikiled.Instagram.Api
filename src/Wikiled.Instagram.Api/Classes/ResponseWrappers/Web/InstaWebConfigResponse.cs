using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebConfigResponse
    {
        [JsonProperty("viewer")] public InstaUserShortResponse Viewer { get; set; }
    }
}
