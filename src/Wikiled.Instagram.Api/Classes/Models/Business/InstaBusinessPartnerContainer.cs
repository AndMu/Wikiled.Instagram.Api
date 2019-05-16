using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaBusinessPartnerContainer : InstaDefault
    {
        [JsonProperty("partners")]
        public InstaBusinessPartner[] Partners { get; set; }
    }
}