using System;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaLocationShortConverter : IObjectConverter<LocationShort, LocationShortResponse>
    {
        public LocationShortResponse SourceObject { get; set; }

        public LocationShort Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var location = new LocationShort
            {
                Name = SourceObject.Name,
                Address = SourceObject.Address,
                ExternalSource = SourceObject.ExternalIdSource,
                ExternalId = SourceObject.ExternalId,
                Lat = SourceObject.Lat,
                Lng = SourceObject.Lng
            };
            return location;
        }
    }
}