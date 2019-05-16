using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class
        InstaDiscoverSearchResultConverter : IObjectConverter<InstaDiscoverSearchResult,
            InstaDiscoverSearchResultResponse>
    {
        public InstaDiscoverSearchResultResponse SourceObject { get; set; }

        public InstaDiscoverSearchResult Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var result = new InstaDiscoverSearchResult
            {
                HasMoreAvailable = SourceObject.HasMore ?? false,
                RankToken = SourceObject.RankToken,
                NumResults = SourceObject.NumResults ?? 0
            };
            if (SourceObject.Users != null && SourceObject.Users.Any())
            {
                foreach (var user in SourceObject.Users)
                {
                    try
                    {
                        result.Users.Add(InstaConvertersFabric.Instance.GetUserConverter(user).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }
    }
}