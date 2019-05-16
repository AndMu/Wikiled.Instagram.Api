using System;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class InstaDiscoverSuggestedSearchesConverter : IObjectConverter<InstaDiscoverSuggestedSearches, InstaDiscoverSuggestedSearchesResponse>
    {
        public InstaDiscoverSuggestedSearchesResponse SourceObject { get; set; }

        public InstaDiscoverSuggestedSearches Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var suggested = new InstaDiscoverSuggestedSearches
                            {
                                RankToken = SourceObject.RankToken
                            };
            if (SourceObject.Suggested != null && SourceObject.Suggested.Any())
            {
                foreach (var search in SourceObject.Suggested)
                {
                    try
                    {
                        suggested.Suggested.Add(ConvertersFabric.Instance.GetDiscoverSearchesConverter(search).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return suggested;
        }
    }
}
