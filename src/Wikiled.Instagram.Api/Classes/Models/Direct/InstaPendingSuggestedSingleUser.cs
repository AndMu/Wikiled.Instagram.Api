using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaPendingSuggestedSingleUser
    {
        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }
    }
}