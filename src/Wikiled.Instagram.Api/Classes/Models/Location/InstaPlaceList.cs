using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Location
{
    public class InstaPlaceList
    {
        public List<long> ExcludeList { get; set; } = new List<long>();

        public bool HasMore { get; set; }

        public List<InstaPlace> Items { get; set; } = new List<InstaPlace>();

        public string RankToken { get; set; }

        internal string Status { get; set; }
    }
}
