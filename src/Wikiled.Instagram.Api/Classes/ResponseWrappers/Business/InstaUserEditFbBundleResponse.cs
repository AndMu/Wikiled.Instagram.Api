using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Business
{
    public class InstaUserEditFbBundleResponse
    {
        [JsonProperty("contact_bar")] public string ContactBar { get; set; }

        [JsonProperty("setting_toggle")] public string SettingToggle { get; set; }

        [JsonProperty("setting_toggle_description")]
        public string SettingToggleDescription { get; set; }
    }
}
