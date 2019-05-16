using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class RankedRecipientResponse
    {
        [JsonProperty("thread")] public RankedRecipientThreadResponse Thread { get; set; }

        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }
    }
}
