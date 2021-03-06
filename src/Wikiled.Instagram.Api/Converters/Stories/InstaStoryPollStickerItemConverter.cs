﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryPollStickerItemConverter : IObjectConverter<InstaStoryPollStickerItem,
            InstaStoryPollStickerItemResponse>
    {
        public InstaStoryPollStickerItemResponse SourceObject { get; set; }

        public InstaStoryPollStickerItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var pollSticker = new InstaStoryPollStickerItem
            {
                Finished = SourceObject.Finished,
                Id = SourceObject.Id,
                IsSharedResult = SourceObject.IsSharedResult,
                PollId = SourceObject.PollId,
                Question = SourceObject.Question,
                ViewerCanVote = SourceObject.ViewerCanVote,
                ViewerVote = SourceObject.ViewerVote ?? 0
            };

            if (SourceObject.Tallies?.Count > 0)
            {
                foreach (var tallies in SourceObject.Tallies)
                {
                    try
                    {
                        pollSticker.Tallies.Add(InstaConvertersFabric.Instance.GetStoryTalliesItemConverter(tallies)
                                                    .Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return pollSticker;
        }
    }
}