using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Story;

namespace Wikiled.Instagram.Api.Classes.Models.Hashtags
{
    public class HashtagStory
    {
        public bool CanReply { get; set; }

        public bool CanReshare { get; set; }

        public DateTime ExpiringAt { get; set; }

        public string Id { get; set; }

        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();

        public int LatestReelMedia { get; set; }

        public bool Muted { get; set; }

        public HashtagOwner Owner { get; set; }

        public int PrefetchCount { get; set; }

        public string ReelType { get; set; }

        public long UniqueIntegerReelId { get; set; }
    }
}