using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastPostLive
    {
        public List<InstaBroadcastInfo> Broadcasts { get; set; } = new List<InstaBroadcastInfo>();

        public int PeakViewerCount { get; set; }

        public string Pk { get; set; }

        public InstaUserShortFriendshipFull User { get; set; }
    }
}
