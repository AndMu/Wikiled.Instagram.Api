﻿using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class
        InstaDirectInboxConverter : IObjectConverter<InstaDirectInboxContainer, InstaDirectInboxContainerResponse>
    {
        public InstaDirectInboxContainerResponse SourceObject { get; set; }

        public InstaDirectInboxContainer Convert()
        {
            var inbox = new InstaDirectInboxContainer
            {
                PendingRequestsCount = SourceObject.PendingRequestsCount,
                SeqId = SourceObject.SeqId,
                SnapshotAt = (SourceObject.SnapshotAtMs ?? 0).FromUnixTimeMiliSeconds()
            };
            if (SourceObject.Subscription != null)
            {
                var converter = InstaConvertersFabric.Instance.GetDirectSubscriptionConverter(SourceObject.Subscription);
                inbox.Subscription = converter.Convert();
            }

            if (SourceObject.Inbox != null)
            {
                inbox.Inbox = new InstaDirectInbox
                {
                    HasOlder = SourceObject.Inbox.HasOlder,
                    UnseenCount = SourceObject.Inbox.UnseenCount,
                    UnseenCountTs = SourceObject.Inbox.UnseenCountTs,
                    OldestCursor = SourceObject.Inbox.OldestCursor,
                    BlendedInboxEnabled = SourceObject.Inbox.BlendedInboxEnabled
                };

                if (SourceObject.Inbox.Threads != null && SourceObject.Inbox.Threads.Count > 0)
                {
                    inbox.Inbox.Threads = new List<InstaDirectInboxThread>();
                    foreach (var inboxThread in SourceObject.Inbox.Threads)
                    {
                        var converter = InstaConvertersFabric.Instance.GetDirectThreadConverter(inboxThread);
                        inbox.Inbox.Threads.Add(converter.Convert());
                    }
                }
            }

            if (SourceObject.PendingUsers == null || SourceObject.PendingUsers.Count <= 0)
            {
                return inbox;
            }

            {
                foreach (var user in SourceObject.PendingUsers)
                {
                    var converter = InstaConvertersFabric.Instance.GetUserShortConverter(user);
                    inbox.PendingUsers.Add(converter.Convert());
                }
            }
            return inbox;
        }
    }
}