using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaTopLive
    {
        public List<InstaUserShortDescriptionFriendshipFull> BroadcastOwners { get; set; } =
            new List<InstaUserShortDescriptionFriendshipFull>();

        public int RankedPosition { get; set; }
    }
}