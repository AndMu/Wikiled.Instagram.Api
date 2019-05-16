using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRecentRecipientsResponse : InstaRecipientsResponse, IInstaRecipientsResponse
    {
        [JsonProperty("recent_recipients")]
        public InstaRankedRecipientResponse[] RankedRecipients { get; set; }
    }
}