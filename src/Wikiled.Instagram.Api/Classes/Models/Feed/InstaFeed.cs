using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Media;
using Wikiled.Instagram.Api.Classes.Models.Other;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.Models.User;

namespace Wikiled.Instagram.Api.Classes.Models.Feed
{
    public class InstaFeed : IInstaBaseList
    {
        public int MediaItemsCount => Medias.Count;

        public List<InstaMedia> Medias { get; set; } = new List<InstaMedia>();

        public List<InstaStory> Stories { get; set; } = new List<InstaStory>();

        public int StoriesItemsCount => Stories.Count;

        public List<InstaSuggestionItem> SuggestedUserItems { get; set; } = new List<InstaSuggestionItem>();

        public string NextMaxId { get; set; }
    }
}