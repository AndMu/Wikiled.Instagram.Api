using System;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaDiscoverSearchesConverter : IObjectConverter<InstaDiscoverSearches, InstaDiscoverSearchesResponse>
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
                               ClientTime = DateTimeHelper.FromUnixTimeSeconds(SourceObject.ClientTime ?? 0),
                               Position = SourceObject.Position,
                               User = ConvertersFabric.Instance.GetUserConverter(SourceObject.User).Convert()
                           };
            return searches;
        }
    }
}
