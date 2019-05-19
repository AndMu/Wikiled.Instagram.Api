using System;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceShortConverter : IObjectConverter<PlaceShort, PlaceShortResponse>
    {
        public PlaceShortResponse SourceObject { get; set; }

        public PlaceShort Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var place = new PlaceShort
            {
                Address = SourceObject.Address,
                City = SourceObject.City,
                ExternalSource = SourceObject.ExternalSource,
                FacebookPlacesId = SourceObject.FacebookPlacesId,
                Lat = SourceObject.Lat,
                Lng = SourceObject.Lng,
                Name = SourceObject.Name,
                Pk = SourceObject.Pk,
                ShortName = SourceObject.ShortName
            };
            return place;
        }
    }
}