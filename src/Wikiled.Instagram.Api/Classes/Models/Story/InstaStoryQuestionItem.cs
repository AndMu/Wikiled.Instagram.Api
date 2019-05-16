namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryQuestionItem
    {
        public float Height { get; set; }

        public int IsHidden { get; set; }

        public int IsPinned { get; set; }

        public InstaStoryQuestionStickerItem QuestionSticker { get; set; } = new InstaStoryQuestionStickerItem();

        public float Rotation { get; set; }

        public float Width { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }
    }
}