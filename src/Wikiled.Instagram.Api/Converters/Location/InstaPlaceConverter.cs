using System;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceConverter : IObjectConverter<InstaPlace, InstaPlaceResponse>
    {
        public InstaPlaceResponse SourceObject { get; set; }

        public InstaPlace Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var place = new InstaPlace
                        {
                            Location = ConvertersFabric.Instance.GetPlaceShortConverter(SourceObject.Location).Convert(),
                            Title = SourceObject.Title,
                            Subtitle = SourceObject.Subtitle
                        };
            return place;
        }
    }
}
