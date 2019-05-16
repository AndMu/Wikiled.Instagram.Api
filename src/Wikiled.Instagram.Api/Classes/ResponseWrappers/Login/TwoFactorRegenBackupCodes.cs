using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    public class InstaTwoFactorRegenBackupCodes
    {
        [JsonProperty("backup_codes")]
        public string[] BackupCodes { get; set; }

        [JsonProperty("status")]
        internal string Status { get; set; }
    }
}