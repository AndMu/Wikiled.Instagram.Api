using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserPresenceResponse
    {
        [JsonProperty("is_active")]
        public bool? IsActive { get; set; }

        [JsonProperty("last_activity_at_ms")]
        public long? LastActivityAtMs { get; set; }

        [JsonIgnore]
        public long Pk { get; set; }
    }
}