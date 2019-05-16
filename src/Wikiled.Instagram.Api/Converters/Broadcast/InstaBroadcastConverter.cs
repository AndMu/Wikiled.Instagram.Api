using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class InstaBroadcastConverter : IObjectConverter<InstaBroadcast, InstaBroadcastResponse>
    {
        public InstaBroadcastResponse SourceObject { get; set; }

        public InstaBroadcast Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcast = new InstaBroadcast
            {
                DashManifest = SourceObject.DashManifest,
                BroadcastMessage = SourceObject.BroadcastMessage,
                BroadcastStatus = SourceObject.BroadcastStatus,
                CoverFrameUrl = SourceObject.CoverFrameUrl,
                DashAbrPlaybackUrl = SourceObject.DashAbrPlaybackUrl,
                DashPlaybackUrl = SourceObject.DashPlaybackUrl,
                Id = SourceObject.Id,
                InternalOnly = SourceObject.InternalOnly,
                MediaId = SourceObject.MediaId,
                OrganicTrackingToken = SourceObject.OrganicTrackingToken,
                PublishedTime = (SourceObject.PublishedTime ?? DateTime.Now.ToUnixTime()).FromUnixTimeSeconds(),
                RtmpPlaybackUrl = SourceObject.RtmpPlaybackUrl,
                ViewerCount = SourceObject.ViewerCount
            };

            if (SourceObject.BroadcastOwner != null)
            {
                broadcast.BroadcastOwner = InstaConvertersFabric.Instance
                    .GetUserShortFriendshipFullConverter(SourceObject.BroadcastOwner)
                    .Convert();
            }

            return broadcast;
        }
    }
}