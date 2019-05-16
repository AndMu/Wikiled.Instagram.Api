﻿using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Challenge;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    internal class InstaLoginTwoFactorBaseResponse
    {
        [JsonProperty("challenge")] public InstaChallengeLoginInfo Challenge { get; set; }

        [JsonProperty("error_type")] public string ErrorType { get; set; }

        [JsonProperty("message")] public string Message { get; set; }
    }
}
