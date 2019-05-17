using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Location;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Location;

namespace Wikiled.Instagram.Api.Converters.Location
{
    internal class InstaLocationSearchConverter : IObjectConverter<InstaLocationShortList, LocationSearchResponse>
    {
        public LocationSearchResponse SourceObject { get; set; }

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
                        InstaConvertersFabric.Instance.GetLocationShortConverter(location).Convert()));
            return locations;
        }
    }
}