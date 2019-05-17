using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaReelFeed
    {
        public bool CanReply { get; set; }

        public bool? CanReshare { get; set; }

        public DateTime ExpiringAt { get; set; }

        public long HasBestiesMedia { get; set; }

        public string Id { get; set; }

        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();

        public long LatestReelMedia { get; set; }

        public long PrefetchCount { get; set; }

        public long Seen { get; set; }

        public InstaUserShortDescriptionFriendshipFull User { get; set; }
    }
}