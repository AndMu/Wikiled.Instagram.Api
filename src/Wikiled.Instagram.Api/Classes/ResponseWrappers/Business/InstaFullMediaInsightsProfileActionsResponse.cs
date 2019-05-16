using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsProfileActionsResponse
    {
        [JsonProperty("actions")]
        public InstaFullMediaInsightsActionsResponse Actions { get; set; }
    }
}