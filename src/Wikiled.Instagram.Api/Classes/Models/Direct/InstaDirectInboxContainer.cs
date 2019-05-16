using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaDirectInboxContainer
    {
        public InstaDirectInbox Inbox { get; set; } = new InstaDirectInbox();

        public int PendingRequestsCount { get; set; }

        public List<InstaUserShort> PendingUsers { get; set; } = new List<InstaUserShort>();

        public int SeqId { get; set; }

        public DateTime SnapshotAt { get; set; }

        public InstaDirectInboxSubscription Subscription { get; set; } = new InstaDirectInboxSubscription();
    }
}