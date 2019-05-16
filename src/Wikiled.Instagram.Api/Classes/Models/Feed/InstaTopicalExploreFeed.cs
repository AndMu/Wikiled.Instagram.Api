using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.TV;

namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaTopicalExploreFeed
    {
        public bool AutoLoadMoreEnabled { get; set; }

        public InstaChannel Channel { get; set; } = new InstaChannel();

        public List<InstaTopicalExploreCluster> Clusters { get; set; } = new List<InstaTopicalExploreCluster>();

        public bool HasShoppingChannelContent { get; set; }

        public string MaxId { get; set; }

        public InstaMediaList Medias { get; set; } = new InstaMediaList();

        public bool MoreAvailable { get; set; }

        public string NextMaxId { get; set; }

        public string RankToken { get; set; }

        public int ResultsCount { get; set; }

        public List<InstaTvChannel> TvChannels { get; set; } = new List<InstaTvChannel>();
    }
}