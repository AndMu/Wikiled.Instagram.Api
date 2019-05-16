using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryUploadOptions
    {
        public InstaStoryCountdownUpload Countdown { get; set; }

        public List<InstaStoryHashtagUpload> Hashtags { get; set; } = new List<InstaStoryHashtagUpload>();

        public List<InstaStoryLocationUpload> Locations { get; set; } = new List<InstaStoryLocationUpload>();

        public List<InstaStoryMentionUpload> Mentions { get; set; } = new List<InstaStoryMentionUpload>();

        public List<InstaStoryPollUpload> Polls { get; set; } = new List<InstaStoryPollUpload>();

        public List<InstaStoryQuestionUpload> Questions { get; set; } = new List<InstaStoryQuestionUpload>();

        public InstaStorySliderUpload Slider { get; set; }

        internal InstaMediaStoryUpload MediaStory { get; set; }
    }
}