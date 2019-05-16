using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class InstaDirectHashtagConverter : IObjectConverter<InstaDirectHashtag, InstaDirectHashtagResponse>
    {
        public InstaDirectHashtagResponse SourceObject { get; set; }

        public InstaDirectHashtag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var hashtag = new InstaDirectHashtag { Name = SourceObject.Name, MediaCount = SourceObject.MediaCount };

            if (SourceObject.Media != null)
            {
                hashtag.Media = InstaConvertersFabric.Instance.GetSingleMediaConverter(SourceObject.Media).Convert();
            }

            return hashtag;
        }
    }
}