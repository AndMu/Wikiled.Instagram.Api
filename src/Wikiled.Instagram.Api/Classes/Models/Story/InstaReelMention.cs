using Wikiled.Instagram.Api.Classes.Models.Hashtags;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaReelMention
    {
        public ApiHashtag Hashtag { get; set; }

        public double Height { get; set; }

        public bool IsHidden { get; set; }

        public bool IsPinned { get; set; }

        public double Rotation { get; set; }

        public UserShortDescription User { get; set; }

        public double Width { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }
}