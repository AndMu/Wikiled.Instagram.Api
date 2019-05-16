using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.BaseResponse;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Broadcast;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Story
{
    public class InstaStoryFeedResponse : InstaBaseStatusResponse
    {
        [JsonProperty("broadcasts")]
        public List<InstaBroadcastResponse> Broadcasts { get; set; }

        [JsonProperty("face_filter_nux_version")]
        public int FaceFilterNuxVersion { get; set; }

        [JsonProperty("has_new_nux_story")]
        public bool HasNewNuxStory { get; set; }

        [JsonProperty("post_live")]
        public InstaBroadcastAddToPostLiveContainerResponse PostLives { get; set; }

        [JsonProperty("sticker_version")]
        public int StickerVersion { get; set; }

        [JsonProperty("story_ranking_token")]
        public string StoryRankingToken { get; set; }

        [JsonProperty("tray")]
        public List< /*InstaReelFeedResponse*/JToken> Tray { get; set; }
    }
}