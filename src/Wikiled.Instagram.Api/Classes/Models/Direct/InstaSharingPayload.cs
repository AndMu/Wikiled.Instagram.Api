using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaSharingPayload
    {
        [JsonProperty("client_context")]
        public object ClientContext { get; set; }

        [JsonProperty("canonical")]
        public bool Cnonical { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("participant_ids")]
        public long[] ParticipantIds { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}