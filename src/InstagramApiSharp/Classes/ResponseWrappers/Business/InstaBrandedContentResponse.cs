using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaBrandedContentResponse
    {
        [JsonProperty("require_approval")] public bool RequireApproval { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("whitelisted_users")] public List<InstaUserShortResponse> WhitelistedUsers { get; set; }
    }
}
