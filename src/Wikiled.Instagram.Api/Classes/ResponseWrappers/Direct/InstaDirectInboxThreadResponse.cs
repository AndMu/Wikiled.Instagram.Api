using System.Collections.Generic;
using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaDirectInboxThreadResponse : InstaBaseStatusResponse
    {
        [JsonProperty("canonical")]
        public bool Canonical { get; set; }

        [JsonProperty("expiring_media_receive_count")]
        public int ExpiringMediaReceiveCount { get; set; }

        [JsonProperty("expiring_media_send_count")]
        public int ExpiringMediaSendCount { get; set; }

        [JsonProperty("has_newer")]
        public bool HasNewer { get; set; }

        [JsonProperty("has_older")]
        public bool HasOlder { get; set; }

        [JsonProperty("inviter")]
        public InstaUserShortResponse Inviter { get; set; }

        [JsonProperty("is_group")]
        public bool IsGroup { get; set; }

        [JsonProperty("is_pin")]
        public bool IsPin { get; set; }

        [JsonProperty("is_spam")]
        public bool IsSpam { get; set; }

        [JsonProperty("items")]
        public List<InstaDirectInboxItemResponse> Items { get; set; }

        [JsonProperty("last_activity_at")]
        public string LastActivity { get; set; }

        [JsonProperty("last_permanent_item")]
        public InstaDirectInboxItemResponse LastPermanentItem { get; set; }

        [JsonProperty("last_seen_at")]
        public object LastSeenAt { get; set; }

        [JsonProperty("left_users")]
        public List<InstaUserShortFriendshipResponse> LeftUsers { get; set; }

        [JsonProperty("mentions_muted")]
        public bool? MentionsMuted { get; set; }

        [JsonProperty("muted")]
        public bool Muted { get; set; }

        [JsonProperty("named")]
        public bool Named { get; set; }

        [JsonProperty("newest_cursor")]
        public string NewestCursor { get; set; }

        [JsonProperty("oldest_cursor")]
        public string OldestCursor { get; set; }

        [JsonProperty("pending")]
        public bool Pending { get; set; }

        [JsonProperty("pending_score")]
        public long? PendingScore { get; set; }

        [JsonProperty("reshare_receive_count")]
        public int ReshareReceiveCount { get; set; }

        [JsonProperty("reshare_send_count")]
        public int ReshareSendCount { get; set; }

        [JsonProperty("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("thread_type")]
        public InstaDirectThreadType ThreadType { get; set; }

        [JsonProperty("thread_title")]
        public string Title { get; set; }

        [JsonProperty("users")]
        public List<InstaUserShortFriendshipResponse> Users { get; set; }

        [JsonProperty("valued_request")]
        public bool ValuedRequest { get; set; }

        [JsonProperty("vc_muted")]
        public bool VcMuted { get; set; }

        [JsonProperty("viewer_id")]
        public string VieweId { get; set; }
    }
}