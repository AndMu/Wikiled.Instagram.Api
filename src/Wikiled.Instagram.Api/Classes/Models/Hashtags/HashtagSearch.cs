using System.Collections.Generic;

namespace Wikiled.Instagram.Api.Classes.Models.Hashtags
{
    public class HashtagSearch : List<Hashtag>
    {
        public bool MoreAvailable { get; set; }

        public string RankToken { get; set; }
    }
}