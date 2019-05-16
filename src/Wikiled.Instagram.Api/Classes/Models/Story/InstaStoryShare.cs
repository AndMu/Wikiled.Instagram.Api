using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryShare
    {
        public bool IsLinked { get; set; }

        public bool IsReelPersisted { get; set; }

        public InstaMedia Media { get; set; }

        public string Message { get; set; }

        public string ReelType { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }
    }
}