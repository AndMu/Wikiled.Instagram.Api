using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Converters.Feeds
{
    internal class InstaTagFeedConverter : IObjectConverter<InstaTagFeed, InstaTagFeedResponse>
    {
        public InstaTagFeedResponse SourceObject { get; set; }

        public InstaTagFeed Convert()
        {
            if (SourceObject?.Medias == null)
            {
                throw new ArgumentNullException("InstaFeedResponse or its media list");
            }

            var feed = new InstaTagFeed();

            List<InstaMedia> ConvertMedia(List<InstaMediaItemResponse> mediasResponse)
            {
                var medias = new List<InstaMedia>();
                foreach (var instaUserFeedItemResponse in mediasResponse)
                {
                    if (instaUserFeedItemResponse?.Type != 0)
                    {
                        continue;
                    }

                    var feedItem = InstaConvertersFabric.Instance.GetSingleMediaConverter(instaUserFeedItemResponse)
                        .Convert();
                    medias.Add(feedItem);
                }

                return medias;
            }

            feed.RankedMedias.AddRange(ConvertMedia(SourceObject.RankedItems));
            feed.Medias.AddRange(ConvertMedia(SourceObject.Medias));
            feed.NextMaxId = SourceObject.NextMaxId;
            foreach (var story in SourceObject.Stories)
            {
                var feedItem = InstaConvertersFabric.Instance.GetStoryConverter(story).Convert();
                feed.Stories.Add(feedItem);
            }

            return feed;
        }
    }
}