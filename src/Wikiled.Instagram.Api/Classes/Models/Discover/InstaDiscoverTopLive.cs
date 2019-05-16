using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverTopLive
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public InstaBroadcastList Broadcasts { get; set; } = new InstaBroadcastList();

        public bool MoreAvailable { get; set; }

        public string NextMaxId { get; set; }

        public List<InstaBroadcastPostLive> PostLiveBroadcasts { get; set; } = new List<InstaBroadcastPostLive>();
    }
}