using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    public class InstaBlockedCommentersResponse : InstaDefault
    {
        [JsonProperty("blocked_commenters")]
        public List<InstaUserShortResponse> BlockedCommenters { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}