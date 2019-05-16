using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Collection
{
    public class InstaCollections
    {
        public List<InstaCollectionItem> Items { get; set; }

        public bool MoreCollectionsAvailable { get; set; }

        public string NextMaxId { get; set; }

        public int Pages { get; set; } = 0;
    }
}