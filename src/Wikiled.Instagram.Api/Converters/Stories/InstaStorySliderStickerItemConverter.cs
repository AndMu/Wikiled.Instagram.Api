﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStorySliderStickerItemConverter : IObjectConverter<InstaStorySliderStickerItem,
            InstaStorySliderStickerItemResponse>
    {
        public InstaStorySliderStickerItemResponse SourceObject { get; set; }

        public InstaStorySliderStickerItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var slider = new InstaStorySliderStickerItem
            {
                Emoji = SourceObject.Emoji,
                Question = SourceObject.Question,
                SliderId = SourceObject.SliderId,
                SliderVoteAverage =
                    SourceObject.SliderVoteAverage == null ? 0 : SourceObject.SliderVoteAverage.Value,
                SliderVoteCount = SourceObject.SliderVoteCount == null ? 0 : SourceObject.SliderVoteCount.Value,
                TextColor = SourceObject.TextColor,
                ViewerCanVote = SourceObject.ViewerCanVote
            };
            return slider;
        }
    }
}