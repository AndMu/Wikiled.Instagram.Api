using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightSingleFeed : InstaHighlightFeed
    {
        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();
    }
}
