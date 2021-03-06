﻿using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Challenge;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Login;

namespace Wikiled.Instagram.Api.Classes.Models.Other
{
    internal class InstaLoginBaseResponse
    {
        [JsonProperty("challenge")]
        public ChallengeLoginInfo Challenge { get; set; }

        [JsonProperty("checkpoint_url")]
        public string CheckpointUrl { get; set; }

        [JsonProperty("lock")]
        public bool? Lock { get; set; }

        [JsonProperty("invalid_credentials")]
        public bool InvalidCredentials { get; set; }

        [JsonProperty("error_type")]
        public string ErrorType { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("help_url")]
        public string HelpUrl { get; set; }

        [JsonProperty("two_factor_required")]
        public bool TwoFactorRequired { get; set; }

        [JsonProperty("two_factor_info")]
        public TwoFactorLoginInfo TwoFactorLoginInfo { get; set; }
    }
}