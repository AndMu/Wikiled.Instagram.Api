using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.User
{
    public class InstaSuggestionItem
    {
        public string Algorithm { get; set; }

        public string Caption { get; set; }

        public string Icon { get; set; }

        public bool IsNewSuggestion { get; set; }

        public List<string> LargeUrls { get; set; } = new List<string>();

        public List<string> MediaIds { get; set; } = new List<string>();

        public List<InstaMedia> MediaInfos { get; set; } = new List<InstaMedia>();

        public string SocialContext { get; set; }

        public List<string> ThumbnailUrls { get; set; } = new List<string>();

        public InstaUserShort User { get; set; }

        public string Uuid { get; set; }

        public float Value { get; set; }
    }
}
