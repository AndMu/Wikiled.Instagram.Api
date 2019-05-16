using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryCountdownList
    {
        public List<InstaStoryCountdownStickerItem> Items { get; set; } = new List<InstaStoryCountdownStickerItem>();

        public string MaxId { get; set; }

        public bool MoreAvailable { get; set; }
    }
}
