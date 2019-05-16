using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsSummaryPromotionsResponse
    {
        [JsonProperty("edges")]
        public object[] Edges { get; set; }
    }
}