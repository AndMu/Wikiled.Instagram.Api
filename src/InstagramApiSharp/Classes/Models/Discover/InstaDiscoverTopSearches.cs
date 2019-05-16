using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverTopSearches
    {
        public string RankToken { get; set; }

        public List<InstaDiscoverSearches> TopResults { get; set; } = new List<InstaDiscoverSearches>();
    }
}
