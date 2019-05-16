using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryItemConverter : IObjectConverter<InstaStoryItem, InstaStoryItemResponse>
    {
        public InstaStoryItemResponse SourceObject { get; set; }

        public InstaStoryItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var instaStory = new InstaStoryItem
            {
                CanViewerSave = SourceObject.CanViewerSave,
                CaptionIsEdited = SourceObject.CaptionIsEdited,
                CaptionPosition = SourceObject.CaptionPosition,
                Code = SourceObject.Code,
                CommentCount = SourceObject.CommentCount,
                ExpiringAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.ExpiringAt),
                FilterType = SourceObject.FilterType,
                HasAudio = SourceObject.HasAudio ?? false,
                HasLiked = SourceObject.HasLiked,
                HasMoreComments = SourceObject.HasMoreComments,
                Id = SourceObject.Id,
                IsReelMedia = SourceObject.IsReelMedia,
                LikeCount = SourceObject.LikeCount,
                MaxNumVisiblePreviewComments = SourceObject.MaxNumVisiblePreviewComments,
                MediaType = SourceObject.MediaType,
                OriginalHeight = SourceObject.OriginalHeight,
                OriginalWidth = SourceObject.OriginalWidth,
                PhotoOfYou = SourceObject.PhotoOfYou,
                Pk = SourceObject.Pk,
                TakenAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.TakenAt),
                ImportedTakenAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject.ImportedTakenAt ?? 0),
                VideoDuration = SourceObject.VideoDuration ?? 0,
                AdAction = SourceObject.AdAction,
                SupportsReelReactions = SourceObject.SupportsReelReactions,
                ShowOneTapTooltip = SourceObject.ShowOneTapTooltip,
                LinkText = SourceObject.LinkText,
                CanReshare = SourceObject.CanReshare,
                CommentLikesEnabled = SourceObject.CommentLikesEnabled,
                CommentThreadingEnabled = SourceObject.CommentThreadingEnabled,
                NumberOfQualities = SourceObject.NumberOfQualities ?? 0,
                TimezoneOffset = SourceObject.TimezoneOffset ?? 0,
                VideoDashManifest = SourceObject.VideoDashManifest,
                StoryIsSavedToArchive = SourceObject.StoryIsSavedToArchive ?? false,
                ViewerCount = SourceObject.ViewerCount ?? 0,
                TotalViewerCount = SourceObject.TotalViewerCount ?? 0,
                ViewerCursor = SourceObject.ViewerCursor,
                HasSharedToFb = SourceObject.HasSharedToFb ?? 0
            };

            if (SourceObject.User != null)
            {
                instaStory.User = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject.User).Convert();
            }

            if (SourceObject.Caption != null)
            {
                instaStory.Caption = InstaConvertersFabric.Instance.GetCaptionConverter(SourceObject.Caption).Convert();
            }

            if (SourceObject.Images?.Candidates != null)
            {
                foreach (var image in SourceObject.Images.Candidates)
                {
                    instaStory.ImageList.Add(
                        new InstaImage(
                            image.Url,
                            int.Parse(image.Width),
                            int.Parse(image.Height)));
                }
            }

            if (SourceObject.VideoVersions != null && SourceObject.VideoVersions.Any())
            {
                foreach (var video in SourceObject.VideoVersions)
                {
                    instaStory.VideoList.Add(
                        new InstaVideo(
                            video.Url,
                            int.Parse(video.Width),
                            int.Parse(video.Height),
                            video.Type));
                }
            }

            if (SourceObject.ReelMentions != null && SourceObject.ReelMentions.Any())
            {
                foreach (var mention in SourceObject.ReelMentions)
                {
                    instaStory.ReelMentions.Add(InstaConvertersFabric.Instance.GetMentionConverter(mention).Convert());
                }
            }

            if (SourceObject.StoryHashtags != null && SourceObject.StoryHashtags.Any())
            {
                foreach (var hashtag in SourceObject.StoryHashtags)
                {
                    instaStory.StoryHashtags.Add(InstaConvertersFabric.Instance.GetMentionConverter(hashtag).Convert());
                }
            }

            if (SourceObject.StoryLocations != null && SourceObject.StoryLocations.Any())
            {
                foreach (var location in SourceObject.StoryLocations)
                {
                    instaStory.StoryLocations.Add(InstaConvertersFabric.Instance.GetStoryLocationConverter(location)
                                                      .Convert());
                }
            }

            if (SourceObject.StoryFeedMedia != null && SourceObject.StoryFeedMedia.Any())
            {
                foreach (var storyFeed in SourceObject.StoryFeedMedia)
                {
                    instaStory.StoryFeedMedia.Add(InstaConvertersFabric.Instance.GetStoryFeedMediaConverter(storyFeed)
                                                      .Convert());
                }
            }

            if (SourceObject.StoryCta != null && SourceObject.StoryCta.Any())
            {
                foreach (var cta in SourceObject.StoryCta)
                {
                    if (cta.Links != null && cta.Links.Any())
                    {
                        foreach (var link in cta.Links)
                        {
                            instaStory.StoryCta.Add(InstaConvertersFabric.Instance.GetStoryCtaConverter(link).Convert());
                        }
                    }
                }
            }

            if (SourceObject.StoryPolls?.Count > 0)
            {
                foreach (var poll in SourceObject.StoryPolls)
                {
                    instaStory.StoryPolls.Add(InstaConvertersFabric.Instance.GetStoryPollItemConverter(poll).Convert());
                }
            }

            if (SourceObject.StorySliders?.Count > 0)
            {
                foreach (var slider in SourceObject.StorySliders)
                {
                    instaStory.StorySliders.Add(InstaConvertersFabric.Instance.GetStorySliderItemConverter(slider)
                                                    .Convert());
                }
            }

            if (SourceObject.StoryQuestions?.Count > 0)
            {
                foreach (var question in SourceObject.StoryQuestions)
                {
                    instaStory.StoryQuestions.Add(InstaConvertersFabric.Instance.GetStoryQuestionItemConverter(question)
                                                      .Convert());
                }
            }

            if (SourceObject.StoryPollVoters?.Count > 0)
            {
                foreach (var voter in SourceObject.StoryPollVoters)
                {
                    instaStory.StoryPollVoters.Add(InstaConvertersFabric.Instance.GetStoryPollVoterInfoItemConverter(voter)
                                                       .Convert());
                }
            }

            if (SourceObject.Viewers?.Count > 0)
            {
                foreach (var viewer in SourceObject.Viewers)
                {
                    instaStory.Viewers.Add(InstaConvertersFabric.Instance.GetUserShortConverter(viewer).Convert());
                }
            }

            if (SourceObject.Likers?.Count > 0)
            {
                foreach (var liker in SourceObject.Likers)
                {
                    instaStory.Likers.Add(InstaConvertersFabric.Instance.GetUserShortConverter(liker).Convert());
                }
            }

            if (SourceObject.PreviewComments?.Count > 0)
            {
                foreach (var comment in SourceObject.PreviewComments)
                {
                    instaStory.PreviewComments.Add(InstaConvertersFabric.Instance.GetCommentConverter(comment).Convert());
                }
            }

            if (SourceObject.StorySliderVoters?.Count > 0)
            {
                foreach (var voter in SourceObject.StorySliderVoters)
                {
                    instaStory.StorySliderVoters.Add(InstaConvertersFabric.Instance
                                                         .GetStorySliderVoterInfoItemConverter(voter)
                                                         .Convert());
                }
            }

            if (SourceObject.StoryQuestionsResponderInfos?.Count > 0)
            {
                foreach (var responderInfo in SourceObject.StoryQuestionsResponderInfos)
                {
                    instaStory.StoryQuestionsResponderInfos.Add(
                        InstaConvertersFabric.Instance.GetStoryQuestionInfoConverter(responderInfo).Convert());
                }
            }

            if (SourceObject.Countdowns?.Count > 0)
            {
                foreach (var countdown in SourceObject.Countdowns)
                {
                    instaStory.Countdowns.Add(InstaConvertersFabric.Instance.GetStoryCountdownItemConverter(countdown)
                                                  .Convert());
                }
            }

            return instaStory;
        }
    }
}