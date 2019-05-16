using System;
using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Enums;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaVisualMedia
    {
        public int Height { get; set; }

        public List<InstaImage> Images { get; set; } = new List<InstaImage>();

        public string InstaIdentifier { get; set; }

        public bool IsExpired => string.IsNullOrEmpty(InstaIdentifier);

        public long MediaId { get; set; }

        public InstaMediaType MediaType { get; set; }

        public string TrackingToken { get; set; }

        public DateTime UrlExpireAt { get; set; }

        public List<InstaVideo> Videos { get; set; } = new List<InstaVideo>();

        public int Width { get; set; }
    }
}
