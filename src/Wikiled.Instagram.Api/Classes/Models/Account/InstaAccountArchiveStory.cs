using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    internal class InstaAccountArchiveStory
    {
        [JsonProperty("message_prefs")]
        public string MessagePrefs { get; set; }

        [JsonProperty("reel_auto_archive")]
        public string ReelAutoArchive { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}