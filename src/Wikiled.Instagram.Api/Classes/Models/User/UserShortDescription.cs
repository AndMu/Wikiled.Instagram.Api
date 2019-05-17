using System;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    [Serializable]
    public class UserShortDescription
    {
        public static UserShortDescription Empty => new UserShortDescription { FullName = string.Empty, UserName = string.Empty };

        public string FullName { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsVerified { get; set; }

        public long Pk { get; set; }

        public string ProfilePicture { get; set; }

        public string ProfilePictureId { get; set; } = "unknown";

        public string ProfilePicUrl { get; set; }

        public string UserName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserShortDescription);
        }

        public override int GetHashCode()
        {
            return Pk.GetHashCode();
        }

        public bool Equals(UserShortDescription user)
        {
            return Pk == user?.Pk;
        }
    }
}