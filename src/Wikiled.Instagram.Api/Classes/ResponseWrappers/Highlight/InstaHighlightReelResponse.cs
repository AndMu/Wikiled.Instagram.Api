using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight
{
    public class InstaHighlightReelResponse : InstaDefault
    {
        [JsonIgnore] public InstaHighlightSingleFeedResponse Reel { get; set; }
    }
}
