using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaSuggestions
    {
        public bool MoreAvailable { get; set; }

        public List<InstaSuggestionItem> NewSuggestedUsers { get; set; } = new List<InstaSuggestionItem>();

        public string NextMaxId { get; set; }

        public List<InstaSuggestionItem> SuggestedUsers { get; set; } = new List<InstaSuggestionItem>();
    }
}
