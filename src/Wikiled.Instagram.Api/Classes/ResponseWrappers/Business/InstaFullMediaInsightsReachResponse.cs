using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaFullMediaInsightsReachResponse
    {
        [JsonProperty("follow_status")] public InstaFullMediaInsightsNodeResponse FollowStatus { get; set; }

        [JsonProperty("value")] public int? Value { get; set; }
    }
}
