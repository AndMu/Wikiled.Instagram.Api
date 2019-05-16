using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaFeed : IInstaBaseList
    {
        public int MediaItemsCount => Medias.Count;

        public List<InstaMedia> Medias { get; set; } = new List<InstaMedia>();

        public string NextMaxId { get; set; }

        public List<InstaStory> Stories { get; set; } = new List<InstaStory>();

        public int StoriesItemsCount => Stories.Count;

        public List<InstaSuggestionItem> SuggestedUserItems { get; set; } = new List<InstaSuggestionItem>();
    }
}
