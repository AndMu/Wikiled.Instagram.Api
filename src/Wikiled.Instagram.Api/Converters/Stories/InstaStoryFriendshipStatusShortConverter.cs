﻿using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryFriendshipStatusShortConverter : IObjectConverter<InstaStoryFriendshipStatusShort,
        InstaStoryFriendshipStatusShortResponse>
    {
        public InstaStoryFriendshipStatusShortResponse SourceObject { get; set; }

        public InstaStoryFriendshipStatusShort Convert()
        {
            var storyFriendshipStatusShort = new InstaStoryFriendshipStatusShort
            {
                Following = SourceObject.Following,
                OutgoingRequest = SourceObject.OutgoingRequest ?? false,
                Muting = SourceObject.Muting ?? false,
                IsMutingReel = SourceObject.IsMutingReel ?? false
            };
            return storyFriendshipStatusShort;
        }
    }
}