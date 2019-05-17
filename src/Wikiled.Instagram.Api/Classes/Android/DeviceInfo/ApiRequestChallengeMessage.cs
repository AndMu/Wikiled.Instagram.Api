using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.Android.DeviceInfo
{
    internal class InstaApiRequestChallengeMessage : ApiRequestMessage
    {
        [JsonProperty("_csrftoken")]
        public string CsrtToken { get; set; }
    }
}