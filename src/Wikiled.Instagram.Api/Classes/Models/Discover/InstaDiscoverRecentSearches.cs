using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverRecentSearches
    {
        public List<InstaDiscoverSearches> Recent { get; set; } = new List<InstaDiscoverSearches>();
    }
}
