using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaTopLive
    {
        public List<InstaUserShortFriendshipFull> BroadcastOwners { get; set; } = new List<InstaUserShortFriendshipFull>();

        public int RankedPosition { get; set; }
    }
}
