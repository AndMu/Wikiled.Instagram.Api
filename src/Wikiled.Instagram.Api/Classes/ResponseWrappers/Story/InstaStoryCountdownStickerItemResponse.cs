﻿using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryCountdownStickerItemResponse
    {
        [JsonProperty("countdown_id")]
        public long CountdownId { get; set; }

        [JsonProperty("digit_card_color")]
        public string DigitCardColor { get; set; }

        [JsonProperty("digit_color")]
        public string DigitColor { get; set; }

        [JsonProperty("end_background_color")]
        public string EndBackgroundColor { get; set; }

        [JsonProperty("end_ts")]
        public long? EndTime { get; set; }

        [JsonProperty("following_enabled")]
        public bool? FollowingEnabled { get; set; }

        [JsonProperty("is_owner")]
        public bool? IsOwner { get; set; }

        [JsonProperty("start_background_color")]
        public string StartBackgroundColor { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("text_color")]
        public string TextColor { get; set; }

        //public object Attribution { get; set; } // Naa moshakhas
        [JsonProperty("viewer_is_following")]
        public bool? ViewerIsFollowing { get; set; }
    }
}