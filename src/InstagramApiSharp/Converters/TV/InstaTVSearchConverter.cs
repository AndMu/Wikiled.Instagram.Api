﻿using System;

namespace Wikiled.Instagram.Api.Converters.TV
{
    internal class InstaTVSearchConverter : IObjectConverter<InstaTVSearch, InstaTVSearchResponse>
    {
        public InstaTVSearchResponse SourceObject { get; set; }

        public InstaTVSearch Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("SourceObject");
            }

            var search = new InstaTVSearch
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
                        search.Results.Add(ConvertersFabric.Instance.GetTVSearchResultConverter(result).Convert());
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
