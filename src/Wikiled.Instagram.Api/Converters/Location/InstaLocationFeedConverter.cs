using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Media;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaLocationFeedConverter : IObjectConverter<InstaLocationFeed, LocationFeedResponse>
    {
        public LocationFeedResponse SourceObject { get; set; }

        public InstaLocationFeed Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            InstaMediaList ConvertMedia(List<InstaMediaItemResponse> mediasResponse)
            {
                var medias = new InstaMediaList();
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

            var feed = new InstaLocationFeed
            {
                MediaCount = SourceObject.MediaCount,
                NextMaxId = SourceObject.NextMaxId,
                Medias = ConvertMedia(SourceObject.Items),
                RankedMedias = ConvertMedia(SourceObject.RankedItems),
                Location = InstaConvertersFabric.Instance.GetLocationConverter(SourceObject.Location).Convert(),
                Story = InstaConvertersFabric.Instance.GetStoryConverter(SourceObject.Story).Convert()
            };
            return feed;
        }
    }
}