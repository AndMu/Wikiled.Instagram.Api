using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaUserChainingContainerResponse : InstaDefault
    {
        [JsonProperty("is_backup")] public bool IsBackup { get; set; }

        [JsonProperty("users")] public List<InstaUserChainingResponse> Users { get; set; }
    }
}
