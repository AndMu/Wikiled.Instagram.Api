using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Story;

namespace Wikiled.Instagram.Api.Classes.Models.Highlight
{
    public class InstaHighlightSingleFeed : InstaHighlightFeed
    {
        public List<InstaStoryItem> Items { get; set; } = new List<InstaStoryItem>();
    }
}