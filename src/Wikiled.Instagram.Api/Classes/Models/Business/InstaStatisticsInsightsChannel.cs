namespace Wikiled.Instagram.Api.Classes.Models.Business
{
    public class InstaStatisticsInsightsChannel
    {
        public string ChannelId { get; set; }

        public string Id { get; set; }

        public object[] Tips { get; set; }

        public int UnseenCount { get; set; } = 0;
    }
}