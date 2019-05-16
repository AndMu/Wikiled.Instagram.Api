using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Hashtags
{
    public class InstaSectionMedia
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public List<InstaMedia> Medias { get; set; } = new List<InstaMedia>();

        public bool MoreAvailable { get; set; }

        public string NextMaxId { get; set; }

        public List<long> NextMediaIds { get; set; } = new List<long>();

        public int NextPage { get; set; }

        public List<InstaRelatedHashtag> RelatedHashtags { get; set; } = new List<InstaRelatedHashtag>();
    }

    /*public class InstaHashtagMedia
    {
        public string LayoutType { get; set; }

        public List<InstaMedia> Medias { get; set; } = new List<InstaMedia>();

        public string FeedType { get; set; }

        public InstaHashtagMediaExploreItemInfo ExploreItemInfo { get; set; }
    }
    public class InstaHashtagMediaExploreItemInfo
    {
        public int NumBolumns { get; set; }

        public int TotalNumBolumns { get; set; }

        public int AspectYatio { get; set; }

        public bool Autoplay { get; set; }
    }*/
}
