using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightFeeds
    {
        public List<InstaHighlightFeed> Items { get; set; } = new List<InstaHighlightFeed>();

        public bool ShowEmptyState { get; set; }

        internal string Status { get; set; }
    }
}
