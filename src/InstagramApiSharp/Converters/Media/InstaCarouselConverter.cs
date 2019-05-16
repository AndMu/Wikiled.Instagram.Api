using System;

namespace Wikiled.Instagram.Api.Converters.Media
{
    internal class InstaCarouselConverter : IObjectConverter<InstaCarousel, InstaCarouselResponse>
    {
        public InstaCarouselResponse SourceObject { get; set; }

        public InstaCarousel Convert()
        {
            var carousel = new InstaCarousel();
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            foreach (var item in SourceObject)
            {
                var carouselItem = ConvertersFabric.Instance.GetCarouselItemConverter(item);
                carousel.Add(carouselItem.Convert());
            }

            return carousel;
        }
    }
}
