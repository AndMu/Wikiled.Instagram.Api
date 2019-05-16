using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaBlockedCommentersResponse : InstaDefault
    {
        [JsonProperty("blocked_commenters")] public List<InstaUserShortResponse> BlockedCommenters { get; set; }

        [JsonProperty("count")] public int Count { get; set; }
    }
}
