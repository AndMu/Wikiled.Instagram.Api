using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaStatisticsImpressionsResponse
    {
        [JsonProperty("organic")]
        public InstaStatisticsOrganicResponse Organic { get; set; }
    }
}