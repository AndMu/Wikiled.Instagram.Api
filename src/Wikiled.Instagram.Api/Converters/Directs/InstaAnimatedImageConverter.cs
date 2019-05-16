using System;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class InstaAnimatedImageConverter : IObjectConverter<InstaAnimatedImage, InstaAnimatedImageResponse>
    {
        public InstaAnimatedImageResponse SourceObject { get; set; }

        public InstaAnimatedImage Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var animatedImage = new InstaAnimatedImage
            {
                Id = SourceObject.Id,
                IsRandom = SourceObject.IsRandom ?? false,
                IsSticker = SourceObject.IsSticker ?? false
            };

            if (SourceObject.Images != null && SourceObject.Images?.Media != null)
            {
                animatedImage.Media = InstaConvertersFabric.Instance
                    .GetAnimatedImageMediaConverter(SourceObject.Images.Media)
                    .Convert();
            }

            if (SourceObject.User != null)
            {
                animatedImage.User =
                    InstaConvertersFabric.Instance.GetAnimatedImageUserConverter(SourceObject.User).Convert();
            }

            return animatedImage;
        }
    }
}