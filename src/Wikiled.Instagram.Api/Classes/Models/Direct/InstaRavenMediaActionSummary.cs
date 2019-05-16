using System;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaRavenMediaActionSummary
    {
        public int Count { get; set; }

        public DateTime ExpireTime { get; set; }

        public InstaRavenType Type { get; set; }
    }
}