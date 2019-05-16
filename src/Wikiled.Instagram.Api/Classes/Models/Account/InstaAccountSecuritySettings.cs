using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountSecuritySettings
    {
        [JsonProperty("backup_codes")] public List<string> BackupCodes { get; set; }

        [JsonProperty("country_code")] public int CountryCode { get; set; }

        [JsonProperty("is_phone_confirmed")] public bool IsPhoneConfirmed { get; set; }

        [JsonProperty("is_two_factor_enabled")]
        public bool IsTwoFactorEnabled { get; set; }

        [JsonProperty("national_number")] public long NationalNumber { get; set; }

        [JsonProperty("phone_number")] public string PhoneNumber { get; set; }

        [JsonProperty("status")] internal string Status { get; set; }
    }
}
