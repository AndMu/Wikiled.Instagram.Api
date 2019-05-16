using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaStorySettings
    {
        [JsonProperty("allow_story_reshare")] public bool AllowStoryReshare { get; set; }

        [JsonProperty("besties")] public InstaAccountBesties Besties { get; set; }

        [JsonProperty("blocked_reels")] public InstaAccountBlockedReels BlockedReels { get; set; }

        /// <summary>
        ///     In dar asl hamon MessagePrefs hast ke tabdil be message replies type shode
        /// </summary>
        [JsonIgnore]
        public InstaMessageRepliesType MessagePrefsType
        {
            get
            {
                switch (MessagePrefs)
                {
                    default:
                    case "everyone":
                        return InstaMessageRepliesType.Everyone;
                    case "following":
                        return InstaMessageRepliesType.Following;
                    case "off":
                        return InstaMessageRepliesType.Off;
                }
            }
        }

        [JsonProperty("persist_stories_to_private_profile")]
        public bool PersistStoriesToPrivateProfile { get; set; }

        [JsonProperty("reel_auto_archive")] public string ReelAutoArchive { get; set; }

        [JsonProperty("save_to_camera_roll")] public bool SaveToCameraRoll { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("message_prefs")] internal string MessagePrefs { get; set; }
    }
}
