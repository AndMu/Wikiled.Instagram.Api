using System;
using Wikiled.Instagram.Api.Classes.Models.Story;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Story;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStorySliderItemConverter : IObjectConverter<InstaStorySliderItem, InstaStorySliderItemResponse>
    {
        public InstaStorySliderItemResponse SourceObject { get; set; }

        public InstaStorySliderItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var slider = new InstaStorySliderItem
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
            if (SourceObject.SliderSticker != null)
            {
                slider.SliderSticker = InstaConvertersFabric.Instance
                    .GetStorySliderStickerItemConverter(SourceObject.SliderSticker)
                    .Convert();
            }

            return slider;
        }
    }
}