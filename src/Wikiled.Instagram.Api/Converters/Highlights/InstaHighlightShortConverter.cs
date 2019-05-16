using System;
using Wikiled.Instagram.Api.Classes.Models.Highlight;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Highlight;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Highlights
{
    internal class InstaHighlightShortConverter : IObjectConverter<InstaHighlightShort, InstaHighlightShortResponse>
    {
        public InstaHighlightShortResponse SourceObject { get; set; }

        public InstaHighlightShort Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var highlight = new InstaHighlightShort
            {
                Id = SourceObject.Id,
                LatestReelMedia = SourceObject.LatestReelMedia,
                MediaCount = SourceObject.MediaCount,
                ReelType = SourceObject.ReelType,
                Time = (SourceObject.Timestamp ?? 0).FromUnixTimeSeconds()
            };
            return highlight;
        }
    }
}