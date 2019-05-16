using System;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightShort
    {
        public string Id { get; set; }

        public int LatestReelMedia { get; set; }

        public int MediaCount { get; set; }

        public string ReelType { get; set; }

        public DateTime Time { get; set; }
    }
}
