﻿using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryTray
    {
        public long Id { get; set; }

        public bool IsPortrait { get; set; }

        public InstaTopLive TopLive { get; set; } = new InstaTopLive();

        public List<InstaStory> Tray { get; set; } = new List<InstaStory>();
    }
}