using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaVisualMediaContainer
    {
        public bool IsExpired
        {
            get
            {
                if (Media != null)
                {
                    return Media.IsExpired;
                }

                return false;
            }
        }

        public InstaVisualMedia Media { get; set; }

        public DateTime ReplayExpiringAtUs { get; set; }

        public int? SeenCount { get; set; }

        public List<long> SeenUserIds { get; set; } = new List<long>();

        public DateTime UrlExpireAt { get; set; }

        public InstaViewMode ViewMode { get; set; }
    }
}