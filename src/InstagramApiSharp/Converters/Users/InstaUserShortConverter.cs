using System;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaUserShortConverter : IObjectConverter<InstaUserShort, InstaUserShortResponse>
    {
        public InstaUserShortResponse SourceObject { get; set; }

        public InstaUserShort Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var user = new InstaUserShort
                       {
                           Pk = SourceObject.Pk,
                           UserName = SourceObject.UserName,
                           FullName = SourceObject.FullName,
                           IsPrivate = SourceObject.IsPrivate,
                           ProfilePicture = SourceObject.ProfilePicture,
                           ProfilePictureId = SourceObject.ProfilePictureId,
                           IsVerified = SourceObject.IsVerified,
                           ProfilePicUrl = SourceObject.ProfilePicture
                       };
            return user;
        }
    }
}
