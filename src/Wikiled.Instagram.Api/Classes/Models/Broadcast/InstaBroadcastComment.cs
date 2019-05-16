namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastComment : InstaBroadcastSendComment
    {
        public int BitFlags { get; set; }

        public bool DidReportAsSpam { get; set; }

        public string InlineComposerDisplayCondition { get; set; }

        public long UserId { get; set; }
    }
}