﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaReelStoryMediaViewersConverter : IObjectConverter<InstaReelStoryMediaViewers,
            InstaReelStoryMediaViewersResponse>
    {
        public InstaReelStoryMediaViewersResponse SourceObject { get; set; }

        public InstaReelStoryMediaViewers Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var reelViewers = new InstaReelStoryMediaViewers
            {
                NextMaxId = SourceObject.NextMaxId,
                TotalScreenshotCount = (int)(SourceObject.TotalScreenshotCount ?? 0),
                TotalViewerCount = (int)(SourceObject.TotalViewerCount ?? 0),
                UserCount = (int)(SourceObject.UserCount ?? 0)
            };

            if (SourceObject.Users?.Count > 0)
            {
                foreach (var user in SourceObject.Users)
                {
                    reelViewers.Users.Add(InstaConvertersFabric.Instance.GetUserShortConverter(user).Convert());
                }
            }

            if (SourceObject.UpdatedMedia != null)
            {
                reelViewers.UpdatedMedia =
                    InstaConvertersFabric.Instance.GetStoryItemConverter(SourceObject.UpdatedMedia).Convert();
            }

            return reelViewers;
        }
    }
}