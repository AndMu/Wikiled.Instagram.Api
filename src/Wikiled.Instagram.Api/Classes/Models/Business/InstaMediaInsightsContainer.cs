using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaMediaInsightsContainer : InstaDefaultResponse
    {
        [JsonProperty("media_organic_insights")]
        public InstaMediaInsights MediaOrganicInsights { get; set; }
    }
}
