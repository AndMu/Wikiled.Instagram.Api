using System;
using System.ComponentModel;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Comment
{
    public class InstaCommentShort : INotifyPropertyChanged
    {
        private bool haslikedcm;

        public int CommentLikeCount { get; set; }

        public InstaContentType ContentType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool HasLikedComment
        {
            get => haslikedcm;
            set
            {
                haslikedcm = value;
                Update("HasLikedComment");
            }
        }

        public long MediaId { get; set; }

        public long ParentCommentId { get; set; }

        public long Pk { get; set; }

        public string Status { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }

        public UserShortDescription User { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Update(string memberName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}