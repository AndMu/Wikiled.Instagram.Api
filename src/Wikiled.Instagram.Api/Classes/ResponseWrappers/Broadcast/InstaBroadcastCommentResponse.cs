using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast
{
    public class InstaBroadcastCommentResponse : InstaBroadcastSendCommentResponse
    {
        [JsonProperty("bit_flags")]
        public int BitFlags { get; set; }

        [JsonProperty("did_report_as_spam")]
        public bool DidReportAsSpam { get; set; }

        [JsonProperty("inline_composer_display_condition")]
        public string InlineComposerDisplayCondition { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}