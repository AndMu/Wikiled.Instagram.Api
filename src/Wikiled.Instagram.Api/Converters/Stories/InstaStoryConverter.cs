﻿using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryConverter : IObjectConverter<InstaStory, InstaStoryResponse>
    {
        public InstaStoryResponse SourceObject { get; set; }

        public InstaStory Convert()
        {
            if (SourceObject == null)
            {
                return null;
            }

            var story = new InstaStory
            {
                CanReply = SourceObject.CanReply,
                ExpiringAt = SourceObject.ExpiringAt.FromUnixTimeSeconds(),
                Id = SourceObject.Id,
                LatestReelMedia = SourceObject.LatestReelMedia,
                Muted = SourceObject.Muted,
                PrefetchCount = SourceObject.PrefetchCount,
                RankedPosition = SourceObject.RankedPosition,
                Seen = (SourceObject.Seen ?? 0).FromUnixTimeSeconds(),
                SeenRankedPosition = SourceObject.SeenRankedPosition,
                SocialContext = SourceObject.SocialContext,
                SourceToken = SourceObject.SourceToken,
                TakenAtUnix = SourceObject.TakenAtUnixLike,
                CanReshare = SourceObject.CanReshare,
                CanViewerSave = SourceObject.CanViewerSave,
                CaptionIsEdited = SourceObject.CaptionIsEdited,
                CaptionPosition = SourceObject.CaptionPosition,
                ClientCacheKey = SourceObject.ClientCacheKey,
                PhotoOfYou = SourceObject.PhotoOfYou,
                IsReelMedia = SourceObject.IsReelMedia,
                VideoDuration = SourceObject.VideoDuration ?? 0,
                SupportsReelReactions = SourceObject.SupportsReelReactions,
                HasSharedToFb = SourceObject.HasSharedToFb,
                ImportedTakenAt = SourceObject.ImportedTakenAt.FromUnixTimeSeconds()
            };

            if (SourceObject.StoryHashtags != null)
            {
                foreach (var item in SourceObject.StoryHashtags)
                {
                    story.StoryHashtags.Add(InstaConvertersFabric.Instance.GetMentionConverter(item).Convert());
                }
            }

            if (SourceObject.StoryLocation != null)
            {
                story.StoryLocation = SourceObject.StoryLocation;
            }

            if (SourceObject.Owner != null)
            {
                story.Owner = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.Owner).Convert();
            }

            if (SourceObject.User != null)
            {
                story.User = InstaConvertersFabric.Instance.GetUserShortFriendshipFullConverter(SourceObject.User).Convert();
            }

            if (SourceObject.Items != null)
            {
                foreach (var item in SourceObject.Items)
                {
                    story.Items.Add(InstaConvertersFabric.Instance.GetStoryItemConverter(item).Convert());
                }
            }

            return story;
        }
    }
}