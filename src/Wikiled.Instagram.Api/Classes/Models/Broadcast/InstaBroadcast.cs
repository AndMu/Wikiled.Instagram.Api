using System;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcast
    {
        public string BroadcastMessage { get; set; }

        public InstaUserShortDescriptionFriendshipFull BroadcastOwner { get; set; }

        public string BroadcastStatus { get; set; }

        public string CoverFrameUrl { get; set; }

        public string DashAbrPlaybackUrl { get; set; }

        public string DashManifest { get; set; }

        public string DashPlaybackUrl { get; set; }

        public string Id { get; set; }

        public bool InternalOnly { get; set; }

        public string MediaId { get; set; }

        public string OrganicTrackingToken { get; set; }

        public DateTime PublishedTime { get; set; }

        public string RtmpPlaybackUrl { get; set; }

        public long ViewerCount { get; set; }
    }
}