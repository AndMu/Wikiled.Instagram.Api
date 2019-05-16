using System.Collections.Generic;
using Wikiled.Instagram.Api.Classes.Models.Broadcast;
using Wikiled.Instagram.Api.Classes.Models.Hashtags;

namespace Wikiled.Instagram.Api.Classes.Models.Story
{
    public class InstaStoryFeed
    {
        public List<InstaBroadcast> Broadcasts { get; set; } = new List<InstaBroadcast>();

        public int FaceFilterNuxVersion { get; set; }

        public List<InstaHashtagStory> HashtagStories { get; set; } = new List<InstaHashtagStory>();

        public bool HasNewNuxStory { get; set; }

        public List<InstaReelFeed> Items { get; set; } = new List<InstaReelFeed>();

        public List<InstaBroadcastAddToPostLive> PostLives { get; set; } = new List<InstaBroadcastAddToPostLive>();

        public int StickerVersion { get; set; }

        public string StoryRankingToken { get; set; }
    }
}