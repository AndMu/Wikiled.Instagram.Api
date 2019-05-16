using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class InstaRelatedHashtagConverter : IObjectConverter<InstaRelatedHashtag, InstaRelatedHashtagResponse>
    {
        public InstaRelatedHashtagResponse SourceObject { get; set; }

        public InstaRelatedHashtag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var relatedHashtag = new InstaRelatedHashtag
            {
                Id = SourceObject.Id, Name = SourceObject.Name, Type = SourceObject.Type
            };
            return relatedHashtag;
        }
    }
}