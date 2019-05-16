using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaTopLiveResponse
    {
        [JsonProperty("broadcast_owners")]
        public List<InstaUserShortFriendshipFullResponse> BroadcastOwners { get; set; } =
            new List<InstaUserShortFriendshipFullResponse>();

        [JsonProperty("ranked_position")]
        public int RankedPosition { get; set; }
    }
}