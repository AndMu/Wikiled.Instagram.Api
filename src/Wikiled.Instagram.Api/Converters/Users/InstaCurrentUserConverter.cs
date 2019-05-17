using System;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaCurrentUserConverter : IObjectConverter<CurrentUser, InstaCurrentUserResponse>
    {
        public InstaCurrentUserResponse SourceObject { get; set; }

        public CurrentUser Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var shortConverter = InstaConvertersFabric.Instance.GetUserShortConverter(SourceObject);
            var user = new CurrentUser(shortConverter.Convert())
            {
                HasAnonymousProfilePicture = SourceObject.HasAnonymousProfilePicture,
                Biography = SourceObject.Biography,
                Birthday = SourceObject.Birthday,
                CountryCode = SourceObject.CountryCode,
                NationalNumber = SourceObject.NationalNumber,
                Email = SourceObject.Email,
                ExternalUrl = SourceObject.ExternalUrl,
                ShowConversionEditEntry = SourceObject.ShowConversationEditEntry,
                Gender = (GenderType)SourceObject.Gender,
                PhoneNumber = SourceObject.PhoneNumber
            };

            if (SourceObject.HdProfilePicVersions != null && SourceObject.HdProfilePicVersions?.Length > 0)
            {
                foreach (var imageResponse in SourceObject.HdProfilePicVersions)
                {
                    var converter = InstaConvertersFabric.Instance.GetImageConverter(imageResponse);
                    user.HdProfileImages.Add(converter.Convert());
                }
            }

            if (SourceObject.HdProfilePicture != null)
            {
                var converter = InstaConvertersFabric.Instance.GetImageConverter(SourceObject.HdProfilePicture);
                user.HdProfilePicture = converter.Convert();
            }

            return user;
        }
    }
}