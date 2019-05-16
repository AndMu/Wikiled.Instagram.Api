using System;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryPollVotersList
    {
        public DateTime LatestPollVoteTime { get; set; }

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }

        public long PollId { get; set; }

        public List<InstaStoryVoterItem> Voters { get; set; } = new List<InstaStoryVoterItem>();
    }
}
