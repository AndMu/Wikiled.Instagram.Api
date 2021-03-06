﻿using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Other
{
    public class InstaPresenceResponse : InstaDefault
    {
        [JsonProperty("disabled")]
        public bool? Disabled { get; set; }

        [JsonProperty("thread_presence_disabled")]
        public bool? ThreadPresenceDisabled { get; set; }
    }
}