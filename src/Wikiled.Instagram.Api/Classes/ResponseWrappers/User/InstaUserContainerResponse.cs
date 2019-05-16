using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserContainerResponse : InstaDefault
    {
        [JsonProperty("user")]
        public InstaUserResponse User { get; set; }
    }
}