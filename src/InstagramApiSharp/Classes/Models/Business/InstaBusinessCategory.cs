using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessCategory
    {
        [JsonProperty("category_id")] public string CategoryId { get; set; }

        [JsonProperty("category_name")] public string CategoryName { get; set; }
    }
}
