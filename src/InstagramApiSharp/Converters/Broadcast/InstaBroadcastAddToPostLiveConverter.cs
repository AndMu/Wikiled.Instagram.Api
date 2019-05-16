﻿using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastAddToPostLiveConverter : IObjectConverter<InstaBroadcastAddToPostLive, InstaBroadcastAddToPostLiveResponse>
    {
        public InstaBroadcastAddToPostLiveResponse SourceObject { get; set; }

        public InstaBroadcastAddToPostLive Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var postlive = new InstaBroadcastAddToPostLive
                           {
                               CanReply = SourceObject.CanReply,
                               LastSeenBroadcastTs = SourceObject.LastSeenBroadcastTs ?? 0,
                               Pk = SourceObject.Pk
                           };

            if (SourceObject.User != null)
            {
                postlive.User = ConvertersFabric.Instance.GetUserShortFriendshipFullConverter(SourceObject.User).Convert();
            }

            if (SourceObject.Broadcasts?.Count > 0)
            {
                postlive.Broadcasts = ConvertersFabric.Instance.GetBroadcastListConverter(SourceObject.Broadcasts).Convert();
            }

            return postlive;
        }
    }
}
