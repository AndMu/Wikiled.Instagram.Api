using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class RelatedHashtagConverter : IObjectConverter<RelatedHashtag, RelatedHashtagResponse>
    {
        public RelatedHashtagResponse SourceObject { get; set; }

        public RelatedHashtag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var relatedHashtag = new RelatedHashtag
            {
                Id = SourceObject.Id, Name = SourceObject.Name, Type = SourceObject.Type
            };

            return relatedHashtag;
        }
    }
}