using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTVChannel
    {
        public bool HasMoreAvailable { get; set; }

        public string Id { get; set; }

        public List<InstaMedia> Items { get; set; } = new List<InstaMedia>();

        public string MaxId { get; set; }

        public string Title { get; set; }

        public InstaTVChannelType Type { get; set; }

        //public Seen_State1 seen_state { get; set; }

        public InstaTVUser UserDetail { get; set; }
    }
}
