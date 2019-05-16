using System;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastLiveHeartBeatViewerCountConverter
        : IObjectConverter<InstaBroadcastLiveHeartBeatViewerCount, InstaBroadcastLiveHeartBeatViewerCountResponse>
    {
        public InstaBroadcastLiveHeartBeatViewerCountResponse SourceObject { get; set; }

        public InstaBroadcastLiveHeartBeatViewerCount Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var heartbeat = new InstaBroadcastLiveHeartBeatViewerCount
                            {
                                BroadcastStatus = SourceObject.BroadcastStatus,
                                CobroadcasterIds = SourceObject.CobroadcasterIds,
                                IsTopLiveEligible = SourceObject.IsTopLiveEligible,
                                OffsetToVideoStart = SourceObject.OffsetToVideoStart,
                                TotalUniqueViewerCount = SourceObject.TotalUniqueViewerCount,
                                ViewerCount = SourceObject.ViewerCount
                            };
            return heartbeat;
        }
    }
}
