using System;

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
                                Time = DateTimeHelper.FromUnixTimeSeconds(SourceObject.Timestamp ?? 0)
                            };
            return highlight;
        }
    }
}
