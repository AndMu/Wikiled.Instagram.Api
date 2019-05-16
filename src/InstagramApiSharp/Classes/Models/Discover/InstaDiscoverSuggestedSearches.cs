using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverSuggestedSearches
    {
        public string RankToken { get; set; }

        public List<InstaDiscoverSearches> Suggested { get; set; } = new List<InstaDiscoverSearches>();
    }
}
