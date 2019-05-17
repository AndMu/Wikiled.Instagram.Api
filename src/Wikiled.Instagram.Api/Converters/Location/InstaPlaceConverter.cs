using System;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceConverter : IObjectConverter<InstaPlace, PlaceResponse>
    {
        public PlaceResponse SourceObject { get; set; }

        public InstaPlace Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var place = new InstaPlace
            {
                Location = InstaConvertersFabric.Instance.GetPlaceShortConverter(SourceObject.Location).Convert(),
                Title = SourceObject.Title,
                Subtitle = SourceObject.Subtitle
            };
            return place;
        }
    }
}