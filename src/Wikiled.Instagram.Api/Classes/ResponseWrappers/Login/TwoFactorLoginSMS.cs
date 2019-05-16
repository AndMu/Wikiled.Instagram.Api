using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    public class InstaTwoFactorLoginSms
    {
        [JsonProperty("two_factor_info")]
        public InstaTwoFactorLogin TwoFactorInfo { get; set; }

        [JsonProperty("two_factor_required")]
        public bool TwoFactorRequired { get; set; }
    }
}