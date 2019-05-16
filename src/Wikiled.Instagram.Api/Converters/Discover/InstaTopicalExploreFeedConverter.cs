using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Feed;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Feed;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class
        InstaTopicalExploreFeedConverter : IObjectConverter<InstaTopicalExploreFeed, InstaTopicalExploreFeedResponse>
    {
        public InstaTopicalExploreFeedResponse SourceObject { get; set; }

        public InstaTopicalExploreFeed Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            List<InstaMedia> ConvertMedia(List<InstaMediaItemResponse> mediasResponse)
            {
                var medias = new List<InstaMedia>();
                if (mediasResponse == null)
                {
                    return medias;
                }

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

            var feed = new InstaTopicalExploreFeed
            {
                NextMaxId = SourceObject.NextMaxId,
                AutoLoadMoreEnabled = SourceObject.AutoLoadMoreEnabled,
                ResultsCount = SourceObject.ResultsCount,
                MoreAvailable = SourceObject.MoreAvailable,
                MaxId = SourceObject.MaxId,
                RankToken = SourceObject.RankToken,
                HasShoppingChannelContent = SourceObject.HasShoppingChannelContent ?? false
            };
            if (SourceObject.TvChannels?.Count > 0)
            {
                foreach (var channel in SourceObject.TvChannels)
                {
                    try
                    {
                        feed.TvChannels.Add(InstaConvertersFabric.Instance.GetTvChannelConverter(channel).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            if (SourceObject.Clusters?.Count > 0)
            {
                foreach (var cluster in SourceObject.Clusters)
                {
                    try
                    {
                        feed.Clusters.Add(InstaConvertersFabric.Instance.GetExploreClusterConverter(cluster).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            if (SourceObject.Channel != null)
            {
                feed.Channel = InstaConvertersFabric.Instance.GetChannelConverter(SourceObject.Channel).Convert();
            }

            feed.Medias.AddRange(ConvertMedia(SourceObject.Medias));
            return feed;
        }
    }
}