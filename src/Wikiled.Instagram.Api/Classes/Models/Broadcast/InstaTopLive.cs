using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaTopLive
    {
        public List<InstaUserShortFriendshipFull> BroadcastOwners { get; set; } =
            new List<InstaUserShortFriendshipFull>();

        public int RankedPosition { get; set; }
    }
}