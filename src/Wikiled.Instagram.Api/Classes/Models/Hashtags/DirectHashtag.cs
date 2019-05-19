using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Hashtags
{
    public class DirectHashtag
    {
        public InstaMedia Media { get; set; }

        public long MediaCount { get; set; }

        public string Name { get; set; }
    }
}