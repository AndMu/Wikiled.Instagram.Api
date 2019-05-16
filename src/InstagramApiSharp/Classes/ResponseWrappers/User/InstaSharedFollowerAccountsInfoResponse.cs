using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.User
{
    public class InstaSharedFollowerAccountsInfoResponse
    {
        [JsonProperty("has_shared_follower_accounts")]
        public bool? HasSharedFollowerAccounts { get; set; }
    }
}
