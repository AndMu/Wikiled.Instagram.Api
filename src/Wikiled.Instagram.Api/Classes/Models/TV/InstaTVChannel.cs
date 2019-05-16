using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTvChannel
    {
        public bool HasMoreAvailable { get; set; }

        public string Id { get; set; }

        public List<InstaMedia> Items { get; set; } = new List<InstaMedia>();

        public string MaxId { get; set; }

        public string Title { get; set; }

        public InstaTvChannelType Type { get; set; }

        //public Seen_State1 seen_state { get; set; }

        public InstaTvUser UserDetail { get; set; }
    }
}