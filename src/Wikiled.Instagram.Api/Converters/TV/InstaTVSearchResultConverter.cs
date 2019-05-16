using System;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTVSearchResultConverter : IObjectConverter<InstaTVSearchResult, InstaTVSearchResultResponse>
    {
        public InstaTVSearchResultResponse SourceObject { get; set; }

        public InstaTVSearchResult Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var search = new InstaTVSearchResult
                         {
                             Type = SourceObject.Type
                         };

            if (SourceObject.Channel != null)
            {
                try
                {
                    search.Channel = ConvertersFabric.Instance.GetTVChannelConverter(SourceObject.Channel).Convert();
                }
                catch
                {
                }
            }

            if (SourceObject.User != null)
            {
                try
                {
                    search.User = ConvertersFabric.Instance.GetUserShortFriendshipConverter(SourceObject.User).Convert();
                }
                catch
                {
                }
            }

            return search;
        }
    }
}
