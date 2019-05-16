﻿using System;

namespace Wikiled.Instagram.Api.Converters.Users
{
    internal class InstaUserPresenceListConverter : IObjectConverter<InstaUserPresenceList, InstaUserPresenceContainerResponse>
    {
        public InstaUserPresenceContainerResponse SourceObject { get; set; }

        public InstaUserPresenceList Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var list = new InstaUserPresenceList();
            if (SourceObject.Items != null && SourceObject.Items.Any())
            {
                foreach (var item in SourceObject.Items)
                {
                    try
                    {
                        list.Add(ConvertersFabric.Instance.GetSingleUserPresenceConverter(item).Convert());
                    }
                    catch
                    {
                    }
                }
            }

            return list;
        }
    }
}
