using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaDirectInboxResponse
    {
        [JsonProperty("blended_inbox_enabled")]
        public bool BlendedInboxEnabled { get; set; }

        [JsonProperty("has_older")]
        public bool HasOlder { get; set; }

        [JsonProperty("oldest_cursor")]
        public string OldestCursor { get; set; }

        [JsonProperty("threads")]
        public List<InstaDirectInboxThreadResponse> Threads { get; set; }

        [JsonProperty("unseen_count")]
        public long UnseenCount { get; set; }

        [JsonProperty("unseen_count_ts")]
        public long UnseenCountTs { get; set; }
    }
}