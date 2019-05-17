using System;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class LocationConverter : IObjectConverter<Classes.Models.Location.Location, LocationResponse>
    {
        public LocationResponse SourceObject { get; set; }

        public Classes.Models.Location.Location Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var location = new Classes.Models.Location.Location
            {
                Name = SourceObject.Name,
                Address = SourceObject.Address,
                City = SourceObject.City,
                ExternalSource = SourceObject.ExternalIdSource,
                ExternalId = SourceObject.ExternalId,
                Lat = SourceObject.Lat,
                Lng = SourceObject.Lng,
                Pk = SourceObject.Pk,
                ShortName = SourceObject.ShortName,
                Height = SourceObject.Height,
                Rotation = SourceObject.Rotation,
                Width = SourceObject.Width,
                X = SourceObject.X,
                Y = SourceObject.Y
            };

            return location;
        }
    }
}