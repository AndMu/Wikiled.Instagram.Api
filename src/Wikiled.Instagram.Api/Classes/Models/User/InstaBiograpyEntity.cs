using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBiograpyEntity
    {
        [JsonProperty("hashtag")]
        public InstaBiographyItem Hashtag { get; set; }

        [JsonProperty("user")]
        public InstaBiographyItem User { get; set; }
    }
}