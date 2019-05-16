using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.TV
{
    public class InstaTvSearch
    {
        public int NumResults { get; set; }

        public string RankToken { get; set; }

        public List<InstaTvSearchResult> Results { get; set; } = new List<InstaTvSearchResult>();

        internal string Status { get; set; }
    }
}