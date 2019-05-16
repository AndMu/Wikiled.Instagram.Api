using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaDirectInboxContainerResponse : BaseStatusResponse
    {
        [JsonProperty("inbox")] public InstaDirectInboxResponse Inbox { get; set; }

        [JsonProperty("pending_requests_total")]
        public int PendingRequestsCount { get; set; }

        [JsonProperty("pending_requests_users")]
        public List<InstaUserShortResponse> PendingUsers { get; set; }

        [JsonProperty("seq_id")] public int SeqId { get; set; }

        [JsonProperty("snapshot_at_ms")] public long? SnapshotAtMs { get; set; }

        [JsonProperty("subscription")] public InstaDirectInboxSubscriptionResponse Subscription { get; set; }
    }
}
