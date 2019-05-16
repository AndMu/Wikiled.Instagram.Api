using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBiographyUser
    {
        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("biography_with_entities")]
        public InstaBiographyEntities BiographyWithEntities { get; set; }

        [JsonProperty("pk")]
        public long Pk { get; set; }
    }
}