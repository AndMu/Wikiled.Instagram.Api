﻿using System;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryVoterItem
    {
        public DateTime Time { get; set; }

        public InstaUserShortFriendship User { get; set; }

        public double Vote { get; set; }
    }
}
