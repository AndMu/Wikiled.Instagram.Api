using System;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastInfo
    {
        public string BroadcastMessage { get; set; }

        public InstaUserShortFriendshipFull BroadcastOwner { get; set; }

        public string BroadcastStatus { get; set; }

        public string CoverFrameUrl { get; set; }

        public string DashManifest { get; set; }

        public string EncodingTag { get; set; }

        public DateTime ExpireAt { get; set; }

        public long Id { get; set; }

        public bool InternalOnly { get; set; }

        public string MediaId { get; set; }

        public int NumberOfQualities { get; set; }

        public string OrganicTrackingToken { get; set; }

        public DateTime PublishedTime { get; set; }

        public int TotalUniqueViewerCount { get; set; }
    }
}
