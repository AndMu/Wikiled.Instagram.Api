using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaMedia : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _cmcount;

        private bool _hasviewersaved;

        private int _likecount;

        private bool _play;

        public bool CanViewerReshare { get; set; }

        public bool CanViewerSave { get; set; }

        public bool CanViewMorePreviewComments { get; set; }

        public InstaCaption Caption { get; set; }

        public bool CaptionIsEdited { get; set; }

        public InstaCarousel Carousel { get; set; }

        public string ClientCacheKey { get; set; }

        public string Code { get; set; }

        public bool CommentLikesEnabled { get; set; }

        public string CommentsCount
        {
            get => _cmcount;
            set
            {
                _cmcount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommentsCount"));
            }
        }

        public bool CommentThreadingEnabled { get; set; }

        public DateTime DeviceTimeStamp { get; set; }

        public bool DirectReplyToAuthorEnabled { get; set; }

        public string FilterType { get; set; }

        public bool HasAudio { get; set; }

        public bool HasLiked
        {
            get => _hasliked;
            set
            {
                _hasliked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasLiked"));
            }
        }

        public bool HasMoreComments { get; set; }

        public bool HasViewerSaved
        {
            get => _hasviewersaved;
            set
            {
                _hasviewersaved = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HasViewerSaved"));
            }
        }

        public string Height { get; set; }

        public List<InstaImage> Images { get; set; } = new List<InstaImage>();

        public string InstaIdentifier { get; set; }

        public bool IsCommentsDisabled { get; set; }

        public bool IsMultiPost => Carousel != null;

        public InstaUserShortList Likers { get; set; } = new InstaUserShortList();

        public int LikesCount
        {
            get => _likecount;
            set
            {
                _likecount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LikesCount"));
            }
        }

        public InstaLocation Location { get; set; }

        public int MaxNumVisiblePreviewComments { get; set; }

        public InstaMediaType MediaType { get; set; }

        public bool NearlyCompleteCopyrightMatch { get; set; }

        public string NextMaxId { get; set; }

        public int NumberOfQualities { get; set; }

        public bool PhotoOfYou { get; set; }

        public string Pk { get; set; }

        /// <summary>
        ///     This property is for developer's personal use.
        /// </summary>
        public bool Play
        {
            get => _play;
            set
            {
                _play = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Play"));
            }
        }

        public List<InstaComment> PreviewComments { get; set; } = new List<InstaComment>();

        public List<InstaProductTag> ProductTags { get; set; } = new List<InstaProductTag>();

        public string ProductType { get; set; }

        public DateTime TakenAt { get; set; }

        public long TakenAtUnix { get; set; }

        public string Title { get; set; }

        public string TrackingToken { get; set; }

        public InstaUser User { get; set; }

        public List<InstaUserTag> UserTags { get; set; } = new List<InstaUserTag>();

        public double VideoDuration { get; set; }

        public List<InstaVideo> Videos { get; set; } = new List<InstaVideo>();

        public int ViewCount { get; set; }

        public int Width { get; set; }

        private bool _hasliked { get; set; }
    }
}
