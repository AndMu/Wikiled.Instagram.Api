namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFriendshipShortStatus
    {
        public bool Following { get; set; }

        public bool IncomingRequest { get; set; }

        public bool IsBestie { get; set; }

        public bool IsPrivate { get; set; }

        public bool OutgoingRequest { get; set; }

        public long Pk { get; set; }
    }
}