using System;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceConverter : IObjectConverter<Place, PlaceResponse>
    {
        public PlaceResponse SourceObject { get; set; }

        public Place Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var place = new Place
            {
                Location = InstaConvertersFabric.Instance.GetPlaceShortConverter(SourceObject.Location).Convert(),
                Title = SourceObject.Title,
                Subtitle = SourceObject.Subtitle
            };
            return place;
        }
    }
}