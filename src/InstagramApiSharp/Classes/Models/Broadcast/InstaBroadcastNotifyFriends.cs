using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastNotifyFriends
    {
        public List<InstaUserShortFriendshipFull> Friends { get; set; } = new List<InstaUserShortFriendshipFull>();

        public int OnlineFriendsCount { get; set; }

        public string Text { get; set; }
    }
}
