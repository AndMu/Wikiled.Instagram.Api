using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Comment
{
    internal class InstaInlineCommentNextIdResponse
    {
        [JsonProperty("bifilter_token")]
        public string BifilterToken { get; set; }

        [JsonProperty("cached_comments_cursor")]
        public string CachedCommentsCursor { get; set; }

        [JsonProperty("server_cursor")]
        public string ServerCursor { get; set; }
    }
}