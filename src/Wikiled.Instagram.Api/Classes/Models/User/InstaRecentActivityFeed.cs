using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaRecentActivityFeed
    {
        public InstaInlineFollow InlineFollow { get; set; }

        public List<InstaLink> Links { get; set; } = new List<InstaLink>();

        public List<InstaActivityMedia> Medias { get; set; } = new List<InstaActivityMedia>();

        public string Pk { get; set; }

        public long ProfileId { get; set; }

        public string ProfileImage { get; set; }

        public string RichText { get; set; }

        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public int Type { get; set; }
    }
}