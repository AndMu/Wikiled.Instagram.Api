using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaInboxMedia
    {
        public List<InstaImage> Images { get; set; } = new List<InstaImage>();

        public InstaMediaType MediaType { get; set; }

        public long OriginalHeight { get; set; }

        public long OriginalWidth { get; set; }

        public List<InstaVideo> Videos { get; set; } = new List<InstaVideo>();
    }
}