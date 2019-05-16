using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    internal class InstaMediaInsightsContainer : InstaDefaultResponse
    {
        [JsonProperty("media_organic_insights")]
        public InstaMediaInsights MediaOrganicInsights { get; set; }
    }
}