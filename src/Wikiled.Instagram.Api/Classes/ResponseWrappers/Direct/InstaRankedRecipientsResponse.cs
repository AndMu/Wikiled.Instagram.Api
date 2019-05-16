using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaRankedRecipientsResponse : InstaRecipientsResponse, IInstaRecipientsResponse
    {
        [JsonProperty("ranked_recipients")] public RankedRecipientResponse[] RankedRecipients { get; set; }
    }
}
