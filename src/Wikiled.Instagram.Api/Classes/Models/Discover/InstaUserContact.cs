using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaUserContact : InstaUserShort
    {
        public string ExtraDisplayName { get; set; }

        public bool HasExtraInfo => !string.IsNullOrEmpty(ExtraDisplayName);
    }
}