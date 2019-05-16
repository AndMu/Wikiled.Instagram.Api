using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsNodeResponse
    {
        [JsonProperty("nodes")]
        public InstaInsightsDataNodeResponse[] Nodes { get; set; }
    }
}