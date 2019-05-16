using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightShortList
    {
        public List<InstaHighlightShort> Items { get; set; } = new List<InstaHighlightShort>();

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }

        public int ResultsCount { get; set; }
    }
}