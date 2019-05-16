using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTv
    {
        public List<InstaTvChannel> Channels { get; set; } = new List<InstaTvChannel>();

        public InstaTvSelfChannel MyChannel { get; set; }

        internal string Status { get; set; }
    }
}