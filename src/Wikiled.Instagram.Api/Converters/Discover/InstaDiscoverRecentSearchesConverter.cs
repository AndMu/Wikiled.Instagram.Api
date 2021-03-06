﻿using System;
using System.Linq;
using Wikiled.Instagram.Api.Classes.Models.Discover;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Discover;

namespace Wikiled.Instagram.Api.Converters.Discover
{
    internal class
        InstaDiscoverRecentSearchesConverter : IObjectConverter<InstaDiscoverRecentSearches,
            InstaDiscoverRecentSearchesResponse>
    {
        public InstaDiscoverRecentSearchesResponse SourceObject { get; set; }

        public InstaDiscoverRecentSearches Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var recents = new InstaDiscoverRecentSearches();
            if (SourceObject.Recent != null && SourceObject.Recent.Any())
            {
                foreach (var search in SourceObject.Recent)
                {
                    try
                    {
                        recents.Recent.Add(InstaConvertersFabric.Instance.GetDiscoverSearchesConverter(search).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return recents;
        }
    }
}