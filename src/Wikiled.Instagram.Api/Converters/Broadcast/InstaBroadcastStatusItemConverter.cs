using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastStatusItemConverter : IObjectConverter<InstaBroadcastStatusItem, InstaBroadcastStatusItemResponse>
    {
        public InstaBroadcastStatusItemResponse SourceObject { get; set; }

        public InstaBroadcastStatusItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastStatusItem = new InstaBroadcastStatusItem
            {
                BroadcastStatus = SourceObject.BroadcastStatus,
                CoverFrameUrl = SourceObject.CoverFrameUrl,
                HasReducedVisibility = SourceObject.HasReducedVisibility,
                Id = SourceObject.Id,
                ViewerCount = SourceObject.ViewerCount
            };

            return broadcastStatusItem;
        }
    }
}