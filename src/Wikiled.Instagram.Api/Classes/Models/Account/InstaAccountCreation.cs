using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Account
{
    public class InstaAccountCreation
    {
        [JsonProperty("account_created")] public bool AccountCreated { get; set; }

        [JsonProperty("created_user")] public InstaUserShortResponse CreatedUser { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
