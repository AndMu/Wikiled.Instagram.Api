namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryQuestionUpload
    {
        public string BackgroundColor { get; set; } = "#ffffff";

        public double Height { get; set; } = 0.32469338000000003;

        public string Question { get; set; }

        public double Rotation { get; set; } = 0.0;

        public string TextColor { get; set; } = "#000000";

        public bool ViewerCanInteract { get; set; } = true;

        public double Width { get; set; } = 0.9507363;

        public double X { get; set; } = 0.5;

        public double Y { get; set; } = 0.5;

        public double Z { get; set; } = 0;

        internal bool IsSticker { get; set; } = true;

        internal string ProfilePicture { get; set; }

        internal string QuestionType { get; set; } = "text";
    }
}