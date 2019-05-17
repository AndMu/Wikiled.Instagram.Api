using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaUserContact : UserShortDescription
    {
        public string ExtraDisplayName { get; set; }

        public bool HasExtraInfo => !string.IsNullOrEmpty(ExtraDisplayName);
    }
}