﻿using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaUserTag
    {
        public InstaPosition Position { get; set; }

        public string TimeInVideo { get; set; }

        public UserShortDescription User { get; set; }
    }
}