using System;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaDiscoverTopLiveConverter : IObjectConverter<InstaDiscoverTopLive, InstaDiscoverTopLiveResponse>
    {
        public InstaDiscoverTopLiveResponse SourceObject { get; set; }

        public InstaDiscoverTopLive Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var discoverTopLive = new InstaDiscoverTopLive
            {
                AutoLoadMoreEnabled = SourceObject.AutoLoadMoreEnabled,
                MoreAvailable = SourceObject.MoreAvailable,
                NextMaxId = SourceObject.NextMaxId
            };

            if (SourceObject.Broadcasts?.Count > 0)
            {
                discoverTopLive.Broadcasts = InstaConvertersFabric.Instance
                    .GetBroadcastListConverter(SourceObject.Broadcasts)
                    .Convert();
            }

            if (SourceObject.PostLiveBroadcasts?.Count > 0)
            {
                foreach (var postLive in SourceObject.PostLiveBroadcasts)
                {
                    discoverTopLive.PostLiveBroadcasts.Add(InstaConvertersFabric.Instance
                                                               .GetBroadcastPostLiveConverter(postLive)
                                                               .Convert());
                }
            }

            return discoverTopLive;
        }
    }
}