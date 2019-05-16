using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaFriendshipShortStatusResponse
    {
        [JsonProperty("following")] public bool Following { get; set; }

        [JsonProperty("incoming_request")] public bool IncomingRequest { get; set; }

        [JsonProperty("is_bestie")] public bool IsBestie { get; set; }

        [JsonProperty("is_private")] public bool IsPrivate { get; set; }

        [JsonProperty("outgoing_request")] public bool OutgoingRequest { get; set; }

        [JsonIgnore] public long Pk { get; set; }
    }
}
