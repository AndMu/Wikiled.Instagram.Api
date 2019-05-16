using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Models.Challenge
{
    public class InstaChallengeLoginInfo
    {
        [JsonProperty("api_path")]
        public string ApiPath { get; set; }

        [JsonProperty("hide_webview_header")]
        public bool HideWebviewHeader { get; set; }

        [JsonProperty("lock")]
        public bool Lock { get; set; }

        [JsonProperty("logout")]
        public bool Logout { get; set; }

        [JsonProperty("native_flow")]
        public bool NativeFlow { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}