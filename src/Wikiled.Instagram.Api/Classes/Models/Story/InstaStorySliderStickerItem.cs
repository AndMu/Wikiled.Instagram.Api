namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStorySliderStickerItem
    {
        public string Emoji { get; set; }

        public string Question { get; set; }

        public long SliderId { get; set; }

        public double SliderVoteAverage { get; set; } = 0;

        public long SliderVoteCount { get; set; } = 0;

        public string TextColor { get; set; }

        public bool ViewerCanVote { get; set; }
    }
}