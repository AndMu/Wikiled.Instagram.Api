using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class InstaHashtagSearchConverter : IObjectConverter<InstaHashtagSearch, InstaHashtagSearchResponse>
    {
        public InstaHashtagSearchResponse SourceObject { get; set; }

        public InstaHashtagSearch Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var tags = new InstaHashtagSearch();

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