using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaTagFeed : InstaFeed
    {
        public List<InstaMedia> RankedMedias { get; set; } = new List<InstaMedia>();
    }
}