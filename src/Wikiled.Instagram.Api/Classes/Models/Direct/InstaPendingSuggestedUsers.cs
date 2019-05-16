using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaPendingSuggestedUsers
    {
        [JsonProperty("netego_type")] public string NetegoType { get; set; }

        [JsonProperty("suggestions")] public InstaPendingSuggestedSingleUser[] Suggestions { get; set; }
    }
}
