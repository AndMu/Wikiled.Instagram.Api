using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaBusinessPartnerContainer : InstaDefault
    {
        [JsonProperty("partners")] public InstaBusinessPartner[] Partners { get; set; }
    }
}
