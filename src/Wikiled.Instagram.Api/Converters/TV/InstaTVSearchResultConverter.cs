using System;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvSearchResultConverter : IObjectConverter<InstaTvSearchResult, InstaTvSearchResultResponse>
    {
        public InstaTvSearchResultResponse SourceObject { get; set; }

        public InstaTvSearchResult Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var search = new InstaTvSearchResult { Type = SourceObject.Type };

            if (SourceObject.Channel != null)
            {
                try
                {
                    search.Channel = InstaConvertersFabric.Instance.GetTvChannelConverter(SourceObject.Channel).Convert();
                }
                catch
                {
                }
            }

            if (SourceObject.User != null)
            {
                try
                {
                    search.User = InstaConvertersFabric.Instance.GetUserShortFriendshipConverter(SourceObject.User)
                        .Convert();
                }
                catch
                {
                }
            }

            return search;
        }
    }
}