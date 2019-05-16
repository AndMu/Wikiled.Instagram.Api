using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessCityLocation
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("__typename")] internal string TypeName { get; set; }
    }
}
