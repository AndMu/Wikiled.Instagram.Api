using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountTwoFactor
    {
        [JsonProperty("backup_codes")] public List<string> BackupCodes { get; set; }

        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("error_type")] internal string ErrorType { get; set; }

        [JsonProperty("status")] internal string Status { get; set; }
    }
}
