using Wikiled.Instagram.Api.Classes.Models.Broadcast;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaDirectBroadcast
    {
        public InstaBroadcast Broadcast { get; set; }

        public bool IsLinked { get; set; }

        public string Message { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }
    }
}