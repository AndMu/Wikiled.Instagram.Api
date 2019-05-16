namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaInlineFollow
    {
        public bool IsFollowing { get; set; }

        public bool IsOutgoingRequest { get; set; }

        public InstaUserShort User { get; set; }
    }
}
