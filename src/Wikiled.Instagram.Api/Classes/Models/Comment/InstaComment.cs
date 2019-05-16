using System;
using System.Collections.Generic;
using System.ComponentModel;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Comment
{
    public class InstaComment : INotifyPropertyChanged
    {
        private bool haslikedcm;

        public int BitFlags { get; set; }

        public int ChildCommentCount { get; set; }

        public InstaContentType ContentType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool DidReportAsSpam { get; set; }

        public bool HasLikedComment
        {
            get => haslikedcm;
            set
            {
                haslikedcm = value;
                Update("HasLikedComment");
            }
        }

        public bool HasMoreHeadChildComments { get; set; }

        //public int NumTailChildComments { get; set; }

        public bool HasMoreTailChildComments { get; set; }

        public int LikesCount { get; set; }

        public List<InstaUserShort> OtherPreviewUsers { get; set; } = new List<InstaUserShort>();

        public long Pk { get; set; }

        //public string NextMaxChildCursor { get; set; }
        public List<InstaCommentShort> PreviewChildComments { get; set; } = new List<InstaCommentShort>();

        public string Status { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }

        public InstaUserShort User { get; set; }

        public long UserId { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(object obj)
        {
            return Equals(obj as InstaComment);
        }

        public override int GetHashCode()
        {
            return Pk.GetHashCode();
        }

        public bool Equals(InstaComment comment)
        {
            return Pk == comment?.Pk;
        }

        private void Update(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }
    }
}