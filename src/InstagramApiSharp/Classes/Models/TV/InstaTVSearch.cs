using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTVSearch
    {
        public int NumResults { get; set; }

        public string RankToken { get; set; }

        public List<InstaTVSearchResult> Results { get; set; } = new List<InstaTVSearchResult>();

        internal string Status { get; set; }
    }
}
