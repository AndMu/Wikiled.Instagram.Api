using Newtonsoft.Json;
using Wikiled.Instagram.Api.Classes.Models.Other;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightReelResponse : InstaDefault
    {
        [JsonIgnore]
        public InstaHighlightSingleFeedResponse Reel { get; set; }
    }
}