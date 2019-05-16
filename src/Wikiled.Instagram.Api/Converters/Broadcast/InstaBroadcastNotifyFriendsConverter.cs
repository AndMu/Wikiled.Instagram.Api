using System;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Converters.Broadcast
{
    internal class
        InstaBroadcastNotifyFriendsConverter : IObjectConverter<InstaBroadcastNotifyFriends,
            InstaBroadcastNotifyFriendsResponse>
    {
        public InstaBroadcastNotifyFriendsResponse SourceObject { get; set; }

        public InstaBroadcastNotifyFriends Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var broadcastNotifyFriends = new InstaBroadcastNotifyFriends
            {
                OnlineFriendsCount = SourceObject.OnlineFriendsCount ?? 0, Text = SourceObject.Text
            };

            try
            {
                if (SourceObject.Friends?.Count > 0)
                {
                    foreach (var friend in SourceObject.Friends)
                    {
                        broadcastNotifyFriends.Friends.Add(
                            InstaConvertersFabric.Instance
                                .GetUserShortFriendshipFullConverter(friend)
                                .Convert());
                    }
                }
            }
            catch
            {
            }

            return broadcastNotifyFriends;
        }
    }
}