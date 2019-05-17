using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStory
    {
        public bool CanReply { get; set; }

        public bool CanReshare { get; set; }

        public bool CanViewerSave { get; set; }

        public bool CaptionIsEdited { get; set; }

        public double? CaptionPosition { get; set; }

        public string ClientCacheKey { get; set; }

        public DateTime ExpiringAt { get; set; }

        public bool HasSharedToFb { get; set; }

        public string Id { get; set; }

        public DateTime ImportedTakenAt { get; set; }

        public bool IsReelMedia { get; set; }

        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();

        public string LatestReelMedia { get; set; }

        public bool Muted { get; set; }

        public UserShortDescription Owner { get; set; }

        public bool PhotoOfYou { get; set; }

        public int PrefetchCount { get; set; }

        public int RankedPosition { get; set; }

        public DateTime Seen { get; set; }

        public int SeenRankedPosition { get; set; }

        public string SocialContext { get; set; }

        public string SourceToken { get; set; }

        public List<InstaReelMention> StoryHashtags { get; set; } = new List<InstaReelMention>();

        public List<InstaStoryLocation> StoryLocation { get; set; } = new List<InstaStoryLocation>();

        public bool SupportsReelReactions { get; set; }

        public long TakenAtUnix { get; set; }

        public InstaUserShortDescriptionFriendshipFull User { get; set; }

        public double VideoDuration { get; set; }
    }
}