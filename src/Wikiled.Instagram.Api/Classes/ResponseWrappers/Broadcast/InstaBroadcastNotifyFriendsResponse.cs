using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastNotifyFriendsResponse
    {
        [JsonProperty("friends")] public List<InstaUserShortFriendshipFullResponse> Friends { get; set; }

        [JsonProperty("online_friends_count")] public int? OnlineFriendsCount { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("text")] public string Text { get; set; }
    }
}
