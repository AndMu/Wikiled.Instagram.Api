using System;
using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStorySliderVoterInfoItem
    {
        public DateTime LatestSliderVoteTime { get; set; }

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }

        public long SliderId { get; set; }

        public List<InstaStoryVoterItem> Voters { get; set; } = new List<InstaStoryVoterItem>();
    }
}
