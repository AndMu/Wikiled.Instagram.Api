﻿using System;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaPlaceShortConverter : IObjectConverter<InstaPlaceShort, InstaPlaceShortResponse>
    {
        public InstaPlaceShortResponse SourceObject { get; set; }

        public InstaPlaceShort Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var place = new InstaPlaceShort
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
