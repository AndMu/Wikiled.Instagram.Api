using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastAddToPostLive
    {
        public InstaBroadcastList Broadcasts { get; set; } = new InstaBroadcastList();

        public bool CanReply { get; set; }

        public double LastSeenBroadcastTs { get; set; }

        public string Pk { get; set; }

        public InstaUserShortFriendshipFull User { get; set; }
    }
}