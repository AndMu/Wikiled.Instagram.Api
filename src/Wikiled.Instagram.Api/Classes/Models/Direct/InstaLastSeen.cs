using System;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaLastSeen : InstaLastSeenItemResponse
    {
        public long PK { get; set; }

        public DateTime SeenTime { get; set; }
    }
}
