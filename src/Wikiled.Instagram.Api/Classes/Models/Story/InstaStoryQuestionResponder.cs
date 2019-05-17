using System;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryQuestionResponder
    {
        public bool HasSharedResponse { get; set; }

        public long Id { get; set; }

        public string ResponseText { get; set; }

        public DateTime Time { get; set; }

        public UserShortDescription User { get; set; }
    }
}