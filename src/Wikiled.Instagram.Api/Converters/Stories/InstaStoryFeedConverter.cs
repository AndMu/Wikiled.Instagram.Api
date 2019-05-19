using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryFeedConverter : IObjectConverter<InstaStoryFeed, InstaStoryFeedResponse>
    {
        public InstaStoryFeedResponse SourceObject { get; set; }

        public InstaStoryFeed Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var feed = new InstaStoryFeed
            {
                FaceFilterNuxVersion = SourceObject.FaceFilterNuxVersion,
                HasNewNuxStory = SourceObject.HasNewNuxStory,
                StickerVersion = SourceObject.StickerVersion,
                StoryRankingToken = SourceObject.StoryRankingToken
            };

            if (SourceObject.Tray != null && SourceObject.Tray.Any())
            {
                foreach (var itemResponse in SourceObject.Tray)
                {
                    var reel = itemResponse.ToObject<InstaReelFeedResponse>();
                    if (reel.Id.ToLower().StartsWith("tag:"))
                    {
                        feed.HashtagStories.Add(
                            InstaConvertersFabric.Instance
                                .GetHashtagStoryConverter(itemResponse.ToObject<HashtagStoryResponse>())
                                .Convert());
                    }
                    else
                    {
                        feed.Items.Add(InstaConvertersFabric.Instance.GetReelFeedConverter(reel).Convert());
                    }
                }
            }

            if (SourceObject.Broadcasts?.Count > 0)
            {
                foreach (var item in SourceObject.Broadcasts)
                {
                    feed.Broadcasts.Add(InstaConvertersFabric.Instance.GetBroadcastConverter(item).Convert());
                }
            }

            if (SourceObject.PostLives?.PostLiveItems?.Count > 0)
            {
                foreach (var postlive in SourceObject.PostLives.PostLiveItems)
                {
                    feed.PostLives.Add(InstaConvertersFabric.Instance.GetAddToPostLiveConverter(postlive).Convert());
                }
            }

            return feed;
        }
    }
}