﻿using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightFeed
    {
        public bool CanReply { get; set; }

        public object CanReshare { get; set; }

        public InstaHighlightCoverMedia CoverMedia { get; set; }

        public string HighlightId { get; set; }

        public int LatestReelMedia { get; set; }

        public int MediaCount { get; set; }

        public int PrefetchCount { get; set; }

        public int RankedPosition { get; set; }

        public string ReelType { get; set; }

        public object Seen { get; set; }

        public int SeenRankedPosition { get; set; }

        public string Title { get; set; }

        public UserShortDescription User { get; set; }
    }
}