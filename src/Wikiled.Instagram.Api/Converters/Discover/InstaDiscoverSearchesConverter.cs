using System;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;
using Wikiled.Instagram.Api.Helpers;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class
        InstaDiscoverSearchesConverter : IObjectConverter<InstaDiscoverSearches, InstaDiscoverSearchesResponse>
    {
        public InstaDiscoverSearchesResponse SourceObject { get; set; }

        public InstaDiscoverSearches Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var searches = new InstaDiscoverSearches
            {
                ClientTime = InstaDateTimeHelper.FromUnixTimeSeconds(SourceObject.ClientTime ?? 0),
                Position = SourceObject.Position,
                User = InstaConvertersFabric.Instance.GetUserConverter(SourceObject.User).Convert()
            };
            return searches;
        }
    }
}