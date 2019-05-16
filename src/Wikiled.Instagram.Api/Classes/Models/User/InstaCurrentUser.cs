using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaCurrentUser : InstaUserShort
    {
        public InstaCurrentUser(InstaUserShort instaUserShort)
        {
            Pk = instaUserShort.Pk;
            UserName = instaUserShort.UserName;
            FullName = instaUserShort.FullName;
            IsPrivate = instaUserShort.IsPrivate;
            ProfilePicture = instaUserShort.ProfilePicture;
            ProfilePictureId = instaUserShort.ProfilePictureId;
            IsVerified = instaUserShort.IsVerified;
        }

        public string Biography { get; set; }

        public string Birthday { get; set; }

        public int CountryCode { get; set; }

        public string Email { get; set; }

        public string ExternalUrl { get; set; }

        public InstaGenderType Gender { get; set; }

        public bool HasAnonymousProfilePicture { get; set; }

        public List<InstaImage> HdProfileImages { get; set; } = new List<InstaImage>();

        public InstaImage HdProfilePicture { get; set; }

        public long NationalNumber { get; set; }

        public string PhoneNumber { get; set; }

        public bool ShowConversionEditEntry { get; set; }
    }
}