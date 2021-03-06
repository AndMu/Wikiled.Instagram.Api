﻿using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Broadcast
{
    public class InstaBroadcastCommentList
    {
        public Caption Caption { get; set; }

        public bool CaptionIsEdited { get; set; }

        public int CommentCount { get; set; }

        public bool CommentLikesEnabled { get; set; }

        public int CommentMuted { get; set; }

        public List<InstaBroadcastComment> Comments { get; set; } = new List<InstaBroadcastComment>();

        public bool HasMoreComments { get; set; }

        public bool HasMoreHeadloadComments { get; set; }

        public string IsFirstFetch { get; set; }

        public int LiveSecondsPerComment { get; set; }

        public string MediaHeaderDisplay { get; set; }

        public InstaBroadcastComment PinnedComment { get; set; }

        public object SystemComments { get; set; }
    }
}