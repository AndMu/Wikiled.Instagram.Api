using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessSuggestedCategory : InstaBusinessCategory
    {
        [JsonProperty("super_cat_id")] public string SuperCatIid { get; set; }

        [JsonProperty("super_cat_name")] public string SuperCatName { get; set; }
    }
}
