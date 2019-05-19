using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagSearchConverter : IObjectConverter<HashtagSearch, HashtagSearchResponse>
    {
        public HashtagSearchResponse SourceObject { get; set; }

        public HashtagSearch Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var tags = new HashtagSearch();

            tags.MoreAvailable = SourceObject.MoreAvailable.GetValueOrDefault(false);
            tags.RankToken = SourceObject.RankToken;
            tags.AddRange(
                SourceObject.Tags.Select(
                    tag =>
                        InstaConvertersFabric.Instance.GetHashTagConverter(tag).Convert()));

            return tags;
        }
    }
}