using System;
using System.ComponentModel;

namespace Wikiled.Instagram.Api.Classes.Models.Comment
{
    public class InstaCommentShort : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _haslikedcm;

        public int CommentLikeCount { get; set; }

        public InstaContentType ContentType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool HasLikedComment
        {
            get => _haslikedcm;
            set
            {
                _haslikedcm = value;
                Update("HasLikedComment");
            }
        }

        public long MediaId { get; set; }

        public long ParentCommentId { get; set; }

        public long Pk { get; set; }

        public string Status { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }

        public InstaUserShort User { get; set; }

        protected void Update(string memberName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
