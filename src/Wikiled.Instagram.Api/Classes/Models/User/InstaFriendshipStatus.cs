﻿namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFriendshipStatus
    {
        public bool Blocking { get; set; }

        public bool FollowedBy { get; set; }

        public bool Following { get; set; }

        public bool IncomingRequest { get; set; }

        public bool IsBlockingReel { get; set; }

        public bool IsPrivate { get; set; }

        public bool OutgoingRequest { get; set; }
    }
}