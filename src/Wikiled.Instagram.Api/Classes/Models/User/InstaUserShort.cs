using System;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    [Serializable]
    public class InstaUserShort
    {
        public static InstaUserShort Empty => new InstaUserShort { FullName = string.Empty, UserName = string.Empty };

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
            return Equals(obj as InstaUserShort);
        }

        public override int GetHashCode()
        {
            return Pk.GetHashCode();
        }

        public bool Equals(InstaUserShort user)
        {
            return Pk == user?.Pk;
        }
    }
}