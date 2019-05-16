using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastInfoConverter : IObjectConverter<InstaBroadcastInfo, InstaBroadcastInfoResponse>
    {
        public InstaBroadcastInfoResponse SourceObject { get; set; }

        public InstaBroadcastInfo Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var unixTime = DateTime.Now.ToUnixTime();
            var broadcastInfo = new InstaBroadcastInfo
            {
                BroadcastMessage = SourceObject.BroadcastMessage,
                BroadcastStatus = SourceObject.BroadcastStatus,
                CoverFrameUrl = SourceObject.CoverFrameUrl,
                DashManifest = SourceObject.DashManifest,
                EncodingTag = SourceObject.EncodingTag,
                Id = SourceObject.Id,
                InternalOnly = SourceObject.InternalOnly,
                MediaId = SourceObject.MediaId,
                NumberOfQualities = SourceObject.NumberOfQualities,
                OrganicTrackingToken = SourceObject.OrganicTrackingToken,
                TotalUniqueViewerCount = SourceObject.TotalUniqueViewerCount,
                ExpireAt = (SourceObject.ExpireAt ?? unixTime).FromUnixTimeSeconds(),
                PublishedTime = (SourceObject.PublishedTime ?? unixTime).FromUnixTimeSeconds()
            };

            if (SourceObject.BroadcastOwner != null)
            {
                broadcastInfo.BroadcastOwner = InstaConvertersFabric.Instance
                    .GetUserShortFriendshipFullConverter(SourceObject.BroadcastOwner)
                    .Convert();
            }

            return broadcastInfo;
        }
    }
}