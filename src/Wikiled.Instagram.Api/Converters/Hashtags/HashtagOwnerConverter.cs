using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class HashtagOwnerConverter : IObjectConverter<HashtagOwner, HashtagOwnerResponse>
    {
        public HashtagOwnerResponse SourceObject { get; set; }

        public HashtagOwner Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var owner = new HashtagOwner
            {
                Name = SourceObject.Name,
                Pk = SourceObject.Pk,
                ProfilePicUrl = SourceObject.ProfilePicUrl,
                ProfilePicUsername = SourceObject.ProfilePicUsername,
                Type = SourceObject.Type
            };

            return owner;
        }
    }
}