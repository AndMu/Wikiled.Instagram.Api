namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaTopicalExploreCluster
    {
        public bool CanMute { get; set; }

        public string Context { get; set; }

        public InstaMedia CoverMedia { get; set; }

        public string DebugInfo { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public bool IsMuted { get; set; }

        public string Name { get; set; }

        public int RankedPosition { get; set; }

        public string Title { get; set; }

        public InstaExploreClusterType Type { get; set; }
    }
}
