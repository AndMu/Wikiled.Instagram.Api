using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    internal class InstaApiRequestChallengeMessage : InstaApiRequestMessage
    {
        [JsonProperty("_csrftoken")]
        public string CsrtToken { get; set; }
    }
}