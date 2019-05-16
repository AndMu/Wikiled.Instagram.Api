using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaBiographyItem
    {
        [JsonProperty("id")] public long Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }
}
