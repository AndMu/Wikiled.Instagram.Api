using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagStoryConverter : IObjectConverter<HashtagStory, HashtagStoryResponse>
    {
        public HashtagStoryResponse SourceObject { get; set; }

        public HashtagStory Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var hashtagStory = new HashtagStory
            {
                CanReply = SourceObject.CanReply,
                CanReshare = SourceObject.CanReshare,
                ExpiringAt = SourceObject.ExpiringAt.FromUnixTimeSeconds(),
                Id = SourceObject.Id,
                LatestReelMedia = SourceObject.LatestReelMedia,
                Muted = SourceObject.Muted,
                PrefetchCount = SourceObject.PrefetchCount,
                ReelType = SourceObject.ReelType,
                UniqueIntegerReelId = SourceObject.UniqueIntegerReelId,
                Owner = InstaConvertersFabric.Instance.GetHashtagOwnerConverter(SourceObject.Owner).Convert()
            };

            try
            {
                foreach (var story in SourceObject.Items)
                {
                    try
                    {
                        hashtagStory.Items.Add(InstaConvertersFabric.Instance.GetStoryItemConverter(story).Convert());
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return hashtagStory;
        }
    }
}