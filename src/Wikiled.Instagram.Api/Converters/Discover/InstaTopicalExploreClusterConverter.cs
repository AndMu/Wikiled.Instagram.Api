using System;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaTopicalExploreClusterConverter : IObjectConverter<InstaTopicalExploreCluster, InstaTopicalExploreClusterResponse>
    {
        public InstaTopicalExploreClusterResponse SourceObject { get; set; }

        public InstaTopicalExploreCluster Convert()
        {
            var cluster = new InstaTopicalExploreCluster
                          {
                              CanMute = SourceObject.CanMute ?? false,
                              Context = SourceObject.Context,
                              DebugInfo = SourceObject.DebugInfo,
                              Description = SourceObject.Description,
                              Id = SourceObject.Id,
                              IsMuted = SourceObject.IsMuted ?? false,
                              Name = SourceObject.Name,
                              RankedPosition = SourceObject.RankedPosition ?? 0,
                              Title = SourceObject.Title
                          };
            try
            {
                var type = SourceObject.Type.Replace("_", "");
                cluster.Type = (InstaExploreClusterType)Enum.Parse(typeof(InstaExploreClusterType), type, true);
            }
            catch
            {
                cluster.Type = InstaExploreClusterType.ExploreAll;
            }

            return cluster;
        }
    }
}
