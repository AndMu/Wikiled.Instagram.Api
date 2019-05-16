using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaPendingSuggestedSingleUser
    {
        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }
    }
}
