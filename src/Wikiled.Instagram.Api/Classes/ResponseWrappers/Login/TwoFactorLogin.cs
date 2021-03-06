﻿using System;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    [Serializable]
    public class TwoFactorLogin
    {
        [JsonProperty("obfuscated_phone_number")]
        public short ObfuscatedPhoneNumber { get; set; }

        [JsonProperty("show_messenger_code_option")]
        public bool ShowMessengerCodeOption { get; set; }

        [JsonProperty("show_new_login_screen")]
        public bool ShowNewLoginScreen { get; set; }

        [JsonProperty("sms_two_factor_on")]
        public bool SmsTwoFactorOn { get; set; }

        [JsonProperty("totp_two_factor_on")]
        public bool TotpTwoFactorOn { get; set; }

        [JsonProperty("two_factor_identifier")]
        public string TwoFactorIdentifier { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}