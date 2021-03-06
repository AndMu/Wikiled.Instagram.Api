﻿using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastNotifyFriends
    {
        public List<InstaUserShortDescriptionFriendshipFull> Friends { get; set; } = new List<InstaUserShortDescriptionFriendshipFull>();

        public int OnlineFriendsCount { get; set; }

        public string Text { get; set; }
    }
}