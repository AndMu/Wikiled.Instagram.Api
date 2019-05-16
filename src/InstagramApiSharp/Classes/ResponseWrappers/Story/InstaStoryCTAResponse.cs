using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryCTAResponse
    {
        [JsonProperty("androidClass")] public string AndroidClass { get; set; }

        [JsonProperty("callToActionTitle")] public string CallToActionTitle { get; set; }

        [JsonProperty("deeplinkUri")] public string DeeplinkUri { get; set; }

        [JsonProperty("igUserId")] public string IgUserId { get; set; }

        [JsonProperty("leadGenFormId")] public string LeadGenFormId { get; set; }

        [JsonProperty("linkType")] public int LinkType { get; set; }

        [JsonProperty("package")] public string Package { get; set; }

        [JsonProperty("redirectUri")] public object RedirectUri { get; set; }

        [JsonProperty("webUri")] public string WebUri { get; set; }
    }
}
