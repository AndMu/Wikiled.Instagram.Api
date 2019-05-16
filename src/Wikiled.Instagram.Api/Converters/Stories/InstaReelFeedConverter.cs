using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaReelFeedConverter : IObjectConverter<InstaReelFeed, InstaReelFeedResponse>
    {
        public InstaReelFeedResponse SourceObject { get; set; }

        public InstaReelFeed Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var reelFeed = new InstaReelFeed
            {
                CanReply = SourceObject.CanReply,
                ExpiringAt = InstaDateTimeHelper.UnixTimestampToDateTime(SourceObject?.ExpiringAt ?? 0),
                HasBestiesMedia = SourceObject.HasBestiesMedia,
                Id = SourceObject.Id,
                LatestReelMedia = SourceObject.LatestReelMedia ?? 0,
                PrefetchCount = SourceObject.PrefetchCount,
                Seen = SourceObject.Seen ?? 0,
                User = InstaConvertersFabric.Instance.GetUserShortFriendshipFullConverter(SourceObject.User).Convert()
            };
            try
            {
                if (!string.IsNullOrEmpty(SourceObject.CanReshare))
                {
                    reelFeed.CanReshare = bool.Parse(SourceObject.CanReshare);
                }
            }
            catch
            {
            }

            if (SourceObject.Items != null && SourceObject.Items.Any())
            {
                foreach (var item in SourceObject.Items)
                {
                    try
                    {
                        reelFeed.Items.Add(InstaConvertersFabric.Instance.GetStoryItemConverter(item).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return reelFeed;
        }
    }
}