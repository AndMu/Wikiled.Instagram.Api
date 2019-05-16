using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsPromotionsUnitResponse
    {
        [JsonProperty("summary_promotions")]
        public InstaStatisticsSummaryPromotionsResponse SummaryPromotions { get; set; }
    }
}