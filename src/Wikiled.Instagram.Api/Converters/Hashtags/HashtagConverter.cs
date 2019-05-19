using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagConverter : IObjectConverter<ApiHashtag, HashtagResponse>
    {
        public HashtagResponse SourceObject { get; set; }

        public ApiHashtag Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var hashtag = new ApiHashtag
            {
                Id = SourceObject.Id,
                Name = SourceObject.Name,
                MediaCount = SourceObject.MediaCount,
                ProfilePicUrl = SourceObject.ProfilePicUrl
            };

            return hashtag;
        }
    }
}