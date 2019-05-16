using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Wikiled.Instagram.Api.Classes.Models.Collection
{
    public class InstaCollectionItem
    {
        public long CollectionId { get; set; }

        public string CollectionName { get; set; }

        public InstaCoverMedia CoverMedia { get; set; }

        public bool HasRelatedMedia { get; set; }

        public InstaMediaList Media { get; set; }

        public string NextMaxId { get; set; }
    }
}