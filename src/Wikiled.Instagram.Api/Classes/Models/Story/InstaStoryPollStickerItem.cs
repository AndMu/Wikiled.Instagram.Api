using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryPollStickerItem
    {
        public bool Finished { get; set; }

        public string Id { get; set; }

        public bool IsSharedResult { get; set; }

        public long PollId { get; set; }

        public string Question { get; set; }

        public List<InstaStoryTalliesItem> Tallies { get; set; } = new List<InstaStoryTalliesItem>();

        public bool ViewerCanVote { get; set; }

        public long ViewerVote { get; set; } = 0;
    }
}