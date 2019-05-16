using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRankedRecipientResponse
    {
        [JsonProperty("thread")]
        public InstaRankedRecipientThreadResponse Thread { get; set; }

        [JsonProperty("user")]
        public InstaUserShortResponse User { get; set; }
    }
}