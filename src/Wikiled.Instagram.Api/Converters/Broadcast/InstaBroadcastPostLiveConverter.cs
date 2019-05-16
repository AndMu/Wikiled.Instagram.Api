using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastPostLiveConverter : IObjectConverter<InstaBroadcastPostLive, InstaBroadcastPostLiveResponse>
    {
        public InstaBroadcastPostLiveResponse SourceObject { get; set; }

        public InstaBroadcastPostLive Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var postLive = new InstaBroadcastPostLive
            {
                PeakViewerCount = SourceObject.PeakViewerCount, Pk = SourceObject.Pk
            };

            if (SourceObject.User != null)
            {
                postLive.User = InstaConvertersFabric.Instance
                    .GetUserShortFriendshipFullConverter(SourceObject.User)
                    .Convert();
            }

            try
            {
                if (SourceObject.Broadcasts?.Count > 0)
                {
                    foreach (var broadcastInfo in SourceObject.Broadcasts)
                    {
                        postLive.Broadcasts.Add(InstaConvertersFabric.Instance.GetBroadcastInfoConverter(broadcastInfo)
                                                    .Convert());
                    }
                }
            }
            catch
            {
            }

            return postLive;
        }
    }
}