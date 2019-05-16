using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class
        InstaFriendshipFullStatusConverter : IObjectConverter<InstaFriendshipFullStatus,
            InstaFriendshipFullStatusResponse>
    {
        public InstaFriendshipFullStatusResponse SourceObject { get; set; }

        public InstaFriendshipFullStatus Convert()
        {
            var friendShip = new InstaFriendshipFullStatus
            {
                Following = SourceObject.Following ?? false,
                Blocking = SourceObject.Blocking ?? false,
                FollowedBy = SourceObject.FollowedBy ?? false,
                OutgoingRequest = SourceObject.OutgoingRequest ?? false,
                IsBestie = SourceObject.IsBestie ?? false,
                Muting = SourceObject.Muting ?? false
            };
            friendShip.IncomingRequest = SourceObject.IncomingRequest ?? false;
            friendShip.IsPrivate = SourceObject.IsPrivate ?? false;
            return friendShip;
        }
    }
}