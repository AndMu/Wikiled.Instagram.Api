using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Login
{
    public class TwoFactorLoginSms
    {
        [JsonProperty("two_factor_info")]
        public TwoFactorLogin TwoFactorInfo { get; set; }

        [JsonProperty("two_factor_required")]
        public bool TwoFactorRequired { get; set; }
    }
}