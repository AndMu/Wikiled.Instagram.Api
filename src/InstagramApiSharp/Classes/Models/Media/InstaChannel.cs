namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaChannel
    {
        public string ChannelId { get; set; }

        public string ChannelType { get; set; }

        public string Context { get; set; }

        public string Header { get; set; }

        public InstaMedia Media { get; set; }

        public string Title { get; set; }
    }
}
