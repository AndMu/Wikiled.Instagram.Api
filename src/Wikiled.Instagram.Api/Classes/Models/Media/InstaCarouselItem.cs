using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Media
{
    public class InstaCarouselItem
    {
        public string CarouselParentId { get; set; }

        public int Height { get; set; }

        public List<InstaImage> Images { get; set; } = new List<InstaImage>();

        public string Identifier { get; set; }

        public InstaMediaType MediaType { get; set; }

        public string Pk { get; set; }

        public List<InstaUserTag> UserTags { get; set; } = new List<InstaUserTag>();

        public List<InstaVideo> Videos { get; set; } = new List<InstaVideo>();

        public int Width { get; set; }
    }
}