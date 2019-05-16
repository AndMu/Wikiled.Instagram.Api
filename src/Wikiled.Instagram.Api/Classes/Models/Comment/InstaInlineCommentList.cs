using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Comment
{
    public class InstaInlineCommentList
    {
        public int ChildCommentCount { get; set; }

        public List<InstaComment> ChildComments { get; set; } = new List<InstaComment>();

        public bool HasMoreHeadChildComments { get; set; }

        public bool HasMoreTailChildComments { get; set; }

        public string NextMaxId { get; set; }

        public string NextMinId { get; set; }

        public int NumTailChildComments { get; set; }

        public InstaComment ParentComment { get; set; }

        internal string Status { get; set; }
    }
}