using System;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Hashtags;

namespace Wikiled.Instagram.Api.Converters.Hashtags
{
    internal class InstaHashtagOwnerConverter : IObjectConverter<InstaHashtagOwner, InstaHashtagOwnerResponse>
    {
        public InstaHashtagOwnerResponse SourceObject { get; set; }

        public InstaHashtagOwner Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var owner = new InstaHashtagOwner
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