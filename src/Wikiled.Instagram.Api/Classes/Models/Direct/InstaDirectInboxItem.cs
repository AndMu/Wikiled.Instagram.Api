using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaDirectInboxItem
    {
        public InstaActionLog ActionLog { get; set; }

        public InstaAnimatedImage AnimatedMedia { get; set; }

        public string ClientContext { get; set; }

        public InstaMedia FelixShareMedia { get; set; }

        public InstaDirectHashtag HashtagMedia { get; set; }

        public string ItemId { get; set; }

        public InstaDirectThreadItemType ItemType { get; set; } = InstaDirectThreadItemType.Text;

        public InstaWebLink LinkMedia { get; set; }

        public InstaDirectBroadcast LiveViewerInvite { get; set; }

        public Location.Location LocationMedia { get; set; }

        public InstaInboxMedia Media { get; set; }

        public InstaMedia MediaShare { get; set; }

        public InstaPlaceholder Placeholder { get; set; }

        public UserShortDescription ProfileMedia { get; set; }

        public List<InstaMedia> ProfileMediasPreview { get; set; }

        public InstaRavenMediaActionSummary RavenExpiringMediaActionSummary { get; set; }

        public InstaVisualMedia RavenMedia { get; set; }

        public int RavenReplayChainCount { get; set; }

        public int RavenSeenCount { get; set; }

        public List<long> RavenSeenUserIds { get; set; } = new List<long>();

        // raven media properties
        public InstaViewMode? RavenViewMode { get; set; }

        public InstaReelShare ReelShareMedia { get; set; }

        public InstaStoryShare StoryShare { get; set; }

        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public long UserId { get; set; }

        public InstaVisualMediaContainer VisualMedia { get; set; }

        public InstaVoiceMedia VoiceMedia { get; set; }
    }
}