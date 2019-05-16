using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.TV;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.TV;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTvSearchConverter : IObjectConverter<InstaTvSearch, InstaTvSearchResponse>
    {
        public InstaTvSearchResponse SourceObject { get; set; }

        public InstaTvSearch Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var search = new InstaTvSearch
            {
                NumResults = SourceObject.NumResults ?? 0,
                Status = SourceObject.Status,
                RankToken = SourceObject.RankToken
            };

            if (SourceObject.Results != null && SourceObject.Results.Any())
            {
                foreach (var result in SourceObject.Results)
                {
                    try
                    {
                        search.Results.Add(InstaConvertersFabric.Instance.GetTvSearchResultConverter(result).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return search;
        }
    }
}