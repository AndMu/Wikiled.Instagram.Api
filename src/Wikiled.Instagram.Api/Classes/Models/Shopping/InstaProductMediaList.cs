using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Shopping
{
    public class InstaProductMediaList
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public List<InstaMedia> Medias { get; set; } = new List<InstaMedia>();

        public bool MoreAvailable { get; set; }

        public string NextMaxId { get; set; }

        public int ResultsCount { get; set; }

        public int TotalCount { get; set; }
    }
}