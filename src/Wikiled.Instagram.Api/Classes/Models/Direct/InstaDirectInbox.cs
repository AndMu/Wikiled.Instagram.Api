using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaDirectInbox
    {
        public bool BlendedInboxEnabled { get; set; }

        public bool HasOlder { get; set; }

        public string OldestCursor { get; set; }

        public List<InstaDirectInboxThread> Threads { get; set; }

        public long UnseenCount { get; set; }

        public long UnseenCountTs { get; set; }
    }
}