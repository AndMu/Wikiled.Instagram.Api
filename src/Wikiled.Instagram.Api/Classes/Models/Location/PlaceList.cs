using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Location
{
    public class PlaceList
    {
        public List<long> ExcludeList { get; set; } = new List<long>();

        public bool HasMore { get; set; }

        public List<Place> Items { get; set; } = new List<Place>();

        public string RankToken { get; set; }

        internal string Status { get; set; }
    }
}