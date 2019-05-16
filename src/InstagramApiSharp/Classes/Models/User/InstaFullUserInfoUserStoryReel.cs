using System;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaFullUserInfoUserStoryReel
    {
        public bool CanReply { get; set; }

        public bool CanReshare { get; set; }

        public DateTime ExpiringAt { get; set; }

        public bool HasBestiesMedia { get; set; }

        public long Id { get; set; }

        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();

        public int? LatestReelMedia { get; set; }

        public int PrefetchCount { get; set; }

        public string ReelType { get; set; }

        public long Seen { get; set; }

        public InstaUserShort User { get; set; }
    }
}
