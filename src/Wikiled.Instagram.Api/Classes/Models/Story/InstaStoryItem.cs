using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Comment;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryItem
    {
        public string AdAction { get; set; }

        public bool CanReshare { get; set; }

        public bool CanViewerSave { get; set; }

        public InstaCaption Caption { get; set; }

        public bool CaptionIsEdited { get; set; }

        public long CaptionPosition { get; set; }

        public string Code { get; set; }

        public long CommentCount { get; set; }

        public bool CommentLikesEnabled { get; set; }

        public bool CommentThreadingEnabled { get; set; }

        public List<InstaStoryCountdownItem> Countdowns { get; set; } = new List<InstaStoryCountdownItem>();

        public DateTime DeviceTimestamp { get; set; }

        public DateTime ExpiringAt { get; set; }

        public long FilterType { get; set; }

        public bool HasAudio { get; set; }

        public bool HasLiked { get; set; }

        public bool HasMoreComments { get; set; }

        public double HasSharedToFb { get; set; }

        public string Id { get; set; }

        public List<InstaImage> ImageList { get; set; } = new List<InstaImage>();

        public DateTime ImportedTakenAt { get; set; }

        public bool IsReelMedia { get; set; }

        public long LikeCount { get; set; }

        public List<UserShortDescription> Likers { get; set; } = new List<UserShortDescription>();

        public string LinkText { get; set; }

        public long MaxNumVisiblePreviewComments { get; set; }

        public long MediaType { get; set; }

        public long NumberOfQualities { get; set; }

        public string OrganicTrackingToken { get; set; }

        public long OriginalHeight { get; set; }

        public long OriginalWidth { get; set; }

        public bool PhotoOfYou { get; set; }

        public long Pk { get; set; }

        public List<InstaComment> PreviewComments { get; set; } = new List<InstaComment>();

        public List<InstaReelMention> ReelMentions { get; set; } = new List<InstaReelMention>();

        public bool ShowOneTapTooltip { get; set; }

        public List<InstaStoryCta> StoryCta { get; set; } = new List<InstaStoryCta>();

        public List<InstaStoryFeedMedia> StoryFeedMedia { get; set; } = new List<InstaStoryFeedMedia>();

        public List<InstaReelMention> StoryHashtags { get; set; } = new List<InstaReelMention>();

        public bool StoryIsSavedToArchive { get; set; }

        public List<InstaStoryLocation> StoryLocations { get; set; } = new List<InstaStoryLocation>();

        public List<InstaStoryPollItem> StoryPolls { get; set; } = new List<InstaStoryPollItem>();

        public List<InstaStoryPollVoterInfoItem> StoryPollVoters { get; set; } =
            new List<InstaStoryPollVoterInfoItem>();

        public List<InstaStoryQuestionItem> StoryQuestions { get; set; } = new List<InstaStoryQuestionItem>();

        public List<InstaStoryQuestionInfo> StoryQuestionsResponderInfos { get; set; } =
            new List<InstaStoryQuestionInfo>();

        public List<InstaStorySliderItem> StorySliders { get; set; } = new List<InstaStorySliderItem>();

        public List<InstaStorySliderVoterInfoItem> StorySliderVoters { get; set; } =
            new List<InstaStorySliderVoterInfoItem>();

        public string StoryStickerIds { get; set; }

        public bool SupportsReelReactions { get; set; }

        public DateTime TakenAt { get; set; }

        public double TimezoneOffset { get; set; }

        public double TotalViewerCount { get; set; }

        public UserShortDescription User { get; set; }

        public string VideoDashManifest { get; set; }

        public double VideoDuration { get; set; }

        public List<InstaVideo> VideoList { get; set; } = new List<InstaVideo>();

        public double ViewerCount { get; set; }

        public string ViewerCursor { get; set; }

        public List<UserShortDescription> Viewers { get; set; } = new List<UserShortDescription>();
    }
}