using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Comment
{
    public class InstaCommentList
    {
        public bool CanViewMorePreviewComments { get; set; }

        public Caption Caption { get; set; }

        public bool CaptionIsEdited { get; set; }

        public List<InstaComment> Comments { get; set; } = new List<InstaComment>();

        public int CommentsCount { get; set; }

        public bool InitiateAtTop { get; set; }

        public bool InsertNewCommentToTop { get; set; }

        public bool LikesEnabled { get; set; }

        public string MediaHeaderDisplay { get; set; }

        public bool MoreCommentsAvailable { get; set; }

        public bool MoreHeadLoadAvailable { get; set; }

        public string NextMaxId { get; set; }

        public string NextMinId { get; set; }

        public List<InstaComment> PreviewComments { get; set; }

        public bool ThreadingEnabled { get; set; }
    }
}