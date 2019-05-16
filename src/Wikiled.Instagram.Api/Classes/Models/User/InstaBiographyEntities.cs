using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBiographyEntities
    {
        [JsonProperty("Entities")]
        public InstaBiograpyEntity[] Entities { get; set; }

        [JsonProperty("nux_type")]
        public string NuxType { get; set; }

        [JsonProperty("raw_text")]
        public string Text { get; set; }
    }
}