﻿using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRankedRecipientThreadResponse
    {
        [JsonProperty("canonical")]
        public bool Canonical { get; set; }

        [JsonProperty("named")]
        public bool Named { get; set; }

        [JsonProperty("pending")]
        public bool Pending { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("thread_title")]
        public string ThreadTitle { get; set; }

        [JsonProperty("thread_type")]
        public string ThreadType { get; set; }

        [JsonProperty("users")]
        public InstaUserShortResponse[] Users { get; set; }

        [JsonProperty("viewer_id")]
        public long ViewerId { get; set; }
    }
}