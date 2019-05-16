﻿using System;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaLocationSearchConverter : IObjectConverter<InstaLocationShortList, InstaLocationSearchResponse>
    {
        public InstaLocationSearchResponse SourceObject { get; set; }

        public InstaLocationShortList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var locations = new InstaLocationShortList();
            locations.AddRange(
                SourceObject.Locations.Select(
                    location =>
                        ConvertersFabric.Instance.GetLocationShortConverter(location).Convert()));
            return locations;
        }
    }
}
