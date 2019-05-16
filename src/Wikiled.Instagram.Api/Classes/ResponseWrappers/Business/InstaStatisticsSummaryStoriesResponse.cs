using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsSummaryStoriesResponse : InstaStatisticsSummaryPromotionsResponse
    {
        [JsonProperty("count")] public long? Count { get; set; } = 0;
    }
}
