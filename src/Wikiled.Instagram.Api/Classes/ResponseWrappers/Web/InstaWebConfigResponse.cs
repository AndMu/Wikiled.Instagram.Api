using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Web
{
    public class InstaWebConfigResponse
    {
        [JsonProperty("viewer")]
        public InstaUserShortResponse Viewer { get; set; }
    }
}