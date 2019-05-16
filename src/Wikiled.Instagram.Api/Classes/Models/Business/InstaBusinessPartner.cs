using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaBusinessPartner
    {
        [JsonProperty("app_id")] public string AppId { get; set; }

        [JsonProperty("label")] public string Label { get; set; }

        [JsonProperty("partner_name")] public string PartnerName { get; set; }
    }
}
