using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Discover
{
    public class InstaDiscoverSearchResult
    {
        public bool HasMoreAvailable { get; set; }

        public int NumResults { get; set; }

        public string RankToken { get; set; }

        public List<InstaUser> Users { get; set; } = new List<InstaUser>();
    }
}