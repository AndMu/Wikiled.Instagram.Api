namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaReelShare
    {
        public bool IsReelPersisted { get; set; }

        public InstaStoryItem Media { get; set; }

        public long ReelOwnerId { get; set; }

        public string ReelType { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }
    }
}