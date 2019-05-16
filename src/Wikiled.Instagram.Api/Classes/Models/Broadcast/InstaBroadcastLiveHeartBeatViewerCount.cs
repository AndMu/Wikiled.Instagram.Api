namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastLiveHeartBeatViewerCount
    {
        public string BroadcastStatus { get; set; }

        public object[] CobroadcasterIds { get; set; }

        public int IsTopLiveEligible { get; set; }

        public int OffsetToVideoStart { get; set; }

        public int TotalUniqueViewerCount { get; set; }

        public float ViewerCount { get; set; }
    }
}
