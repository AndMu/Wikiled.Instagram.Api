using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class CurrentUser : UserShortDescription
    {
        public CurrentUser(UserShortDescription instaUserShortDescription)
        {
            Pk = instaUserShortDescription.Pk;
            UserName = instaUserShortDescription.UserName;
            FullName = instaUserShortDescription.FullName;
            IsPrivate = instaUserShortDescription.IsPrivate;
            ProfilePicture = instaUserShortDescription.ProfilePicture;
            ProfilePictureId = instaUserShortDescription.ProfilePictureId;
            IsVerified = instaUserShortDescription.IsVerified;
        }

        public string Biography { get; set; }

        public string Birthday { get; set; }

        public int CountryCode { get; set; }

        public string Email { get; set; }

        public string ExternalUrl { get; set; }

        public GenderType Gender { get; set; }

        public bool HasAnonymousProfilePicture { get; set; }

        public List<InstaImage> HdProfileImages { get; set; } = new List<InstaImage>();

        public InstaImage HdProfilePicture { get; set; }

        public long NationalNumber { get; set; }

        public string PhoneNumber { get; set; }

        public bool ShowConversionEditEntry { get; set; }
    }
}