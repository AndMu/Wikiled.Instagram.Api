using System;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryQuestionInfo
    {
        public string BackgroundColor { get; set; }

        public DateTime LatestQuestionResponseTime { get; set; }

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }

        public string Question { get; set; }

        public long QuestionId { get; set; }

        public int QuestionResponseCount { get; set; }

        public string QuestionType { get; set; }

        public List<InstaStoryQuestionResponder> Responders { get; set; } = new List<InstaStoryQuestionResponder>();

        public string TextColor { get; set; }
    }
}