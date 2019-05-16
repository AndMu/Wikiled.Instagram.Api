using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class
        InstaStoryCountdownItemConverter : IObjectConverter<InstaStoryCountdownItem, InstaStoryCountdownItemResponse>
    {
        public InstaStoryCountdownItemResponse SourceObject { get; set; }

        public InstaStoryCountdownItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var storyCountdownItem = new InstaStoryCountdownItem
            {
                Height = SourceObject.Height,
                IsHidden = SourceObject.IsHidden,
                IsPinned = SourceObject.IsPinned,
                Rotation = SourceObject.Rotation,
                Width = SourceObject.Width,
                X = SourceObject.X,
                Y = SourceObject.Y,
                Z = SourceObject.Z
            };

            if (SourceObject.CountdownSticker != null)
            {
                storyCountdownItem.CountdownSticker = InstaConvertersFabric.Instance
                    .GetStoryCountdownStickerItemConverter(SourceObject.CountdownSticker)
                    .Convert();
            }

            return storyCountdownItem;
        }
    }
}